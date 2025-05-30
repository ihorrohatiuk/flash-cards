using Blazored.SessionStorage;
using FlashCards.WebUI;
using FlashCards.WebUI.Handlers;
using FlashCards.WebUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddTransient<AuthenticationHandler>();
builder.Services.AddScoped<AiService>();

builder.Services.AddScoped(sp => 
    new HttpClient
    {
        // using to getting local files like sample-data
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    });
/*Backend url*/
builder.Services.AddHttpClient("ServerApi")
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? ""))
    .AddHttpMessageHandler<AuthenticationHandler>();

// Authentication
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
// Flash cards state container to pass flash cards between /ai-flashcards and /add-unit pages
builder.Services.AddScoped<FlashCardStateContainer>();
// Session storage
builder.Services.AddBlazoredSessionStorageAsSingleton();
// MudBlazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();