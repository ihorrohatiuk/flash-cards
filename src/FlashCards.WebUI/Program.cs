using Blazored.SessionStorage;
using FlashCards.WebUI;
using FlashCards.WebUI.Handlers;
using FlashCards.WebUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AuthenticationHandler>();

builder.Services.AddScoped(sp => 
    new HttpClient
    {
        // using to getting local files like sample-data
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    });
builder.Services.AddHttpClient("ServerApi")
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? ""))
    .AddHttpMessageHandler<AuthenticationHandler>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddMudServices();

await builder.Build().RunAsync();