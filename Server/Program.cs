using Ceres.Server.Games;
using Ceres.Server.Hubs;
using Ceres.Server.Services;
using Newtonsoft.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(hubOptions => {
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(5);
}).AddNewtonsoftJsonProtocol(options => {
    options.PayloadSerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
    // ((DefaultContractResolver)options.PayloadSerializerSettings.ContractResolver).IgnoreSerializableAttribute = false;
});

builder.Services.AddSingleton<CardDatabaseLoader>();
builder.Services.AddSingleton<CardDeckLoader>();
builder.Services.AddSingleton<ISignalRService, SignalRService>();
builder.Services.AddSingleton<IBattleService, BattleService>();
builder.Services.AddSingleton<IServerBattleManager, ServerBattleManager>();



var app = builder.Build();

app.Services.GetRequiredService<IBattleService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// app.UseAuthorization();
app.MapRazorPages();

app.MapHub<LobbyHub>("/LobbyHub");
app.MapHub<GameHub>("/GameHub");

// Console.WriteLine(typeof(TestDrawCommand).FullName);
// Console.WriteLine(typeof(TestDrawCommand).Assembly.GetName().Name);

app.Run();
