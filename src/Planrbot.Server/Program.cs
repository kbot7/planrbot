using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

using Planrbot.Server.Data;

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
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MainDbContext>(
		options => options.UseInMemoryDatabase("planrbot"));

//builder.Services.AddDbContext<MainDbContext>(
//		options => options.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

var app = builder.Build();

// Migrate db
if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
		//await db.Database.EnsureDeletedAsync();
		//await db.Database.MigrateAsync();
		var seeder = new DataSeeder(db);
		await seeder.SeedAsync();
	}
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();