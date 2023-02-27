using Ceres.Server.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(hubOptions => {
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(5);
}).AddNewtonsoftJsonProtocol(options => {
    options.PayloadSerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
});

builder.Services.AddSingleton<CardDatabaseLoader>();
builder.Services.AddSingleton<CardDeckLoader>();
builder.Services.AddScoped<IBattleService, BattleService>();
builder.Services.AddSingleton<IServerBattleManager, ServerBattleManager>();
builder.Services.AddSingleton<ISignalRService, SignalRService>();


var app = builder.Build();

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

app.Run();
