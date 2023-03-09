using System.Text.Json.Serialization;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
//		.EnableTokenAcquisitionToCallDownstreamApi()
//			.AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
//			.AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
	opt.MapType<DateOnly>(() => new OpenApiSchema
	{
		Type = "string",
		Format = "date"
	});
});


if (builder.Environment.IsProduction())
{
	builder.Services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Main")));
}
else
{
	builder.Services.AddDbContext<MainDbContext>(opt => opt.UseInMemoryDatabase("planr"));
}

builder.Services.Configure<JsonOptions>(options =>
{
	options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});



//builder.Services.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

var app = builder.Build();

// Seed dev DB
if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
		var seeder = new DataSeeder(db);
		await seeder.SeedAsync();
	}
}

bool swaggerEnabled = app.Configuration.GetValue<bool>("PLANRBOT_SWAGGER_ENABLED") || app.Environment.IsDevelopment();
if (swaggerEnabled)
{
	app.UseSwagger();
}

bool swaggerUIEnabled = (app.Configuration.GetValue<bool>("PLANRBOT_SWAGGER_UI_ENABLED") && swaggerEnabled) || app.Environment.IsDevelopment();
if (swaggerUIEnabled)
{
	app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

bool blazorEnabled = app.Configuration.GetValue<bool>("PLANRBOT_BLAZOR_ENABLED");
if (blazorEnabled)
{
	app.UseWebAssemblyDebugging();
	app.UseBlazorFrameworkFiles();
}

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();