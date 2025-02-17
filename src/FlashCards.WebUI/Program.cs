using Blazored.SessionStorage;
using FlashCards.WebUI;
using FlashCards.WebUI.Handllers;
using FlashCards.WebUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AuthenticationHandller>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//TODO: change after authentication will be added (include Units.razor)
builder.Services.AddHttpClient("ServerApi")
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ServerUrl"] ?? ""))
    .AddHttpMessageHandler<AuthenticationHandller>();

builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredSessionStorageAsSingleton();
builder.Services.AddMudServices();

await builder.Build().RunAsync();