using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.States;
using web_client;
using webclient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<LobbyHub>();
builder.Services.AddScoped<GameHub>();
builder.Services.AddSingleton<UserState>();

await builder.Build().RunAsync();
