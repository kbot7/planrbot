using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Planrbot.Web.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
//.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Planrbot.Web.ServerAPI"));

builder.Services.AddMudServices(options =>
{
	options.PopoverOptions.ThrowOnDuplicateProvider = false;

	options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
	options.SnackbarConfiguration.PreventDuplicates = false;
	options.SnackbarConfiguration.NewestOnTop = false;
	options.SnackbarConfiguration.ShowCloseIcon = true;
	options.SnackbarConfiguration.VisibleStateDuration = 10000;
	options.SnackbarConfiguration.HideTransitionDuration = 500;
	options.SnackbarConfiguration.ShowTransitionDuration = 500;
	options.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddFluxor(opt =>
{
	opt.ScanAssemblies(typeof(Program).Assembly);
	opt.UseRouting();
	opt.UseReduxDevTools();
});

//builder.Services.AddMsalAuthentication(options =>
//{
//	builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
//	options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetSection("ServerApi")["Scopes"]);
//});

await builder.Build().RunAsync();