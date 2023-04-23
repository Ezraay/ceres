using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;


namespace webclient.Services;


public class LobbyHub : ISignalRHub
{
    public HubConnection LobbyHubConnection { get; }
    public event Action? OnHubStataHasChanged;
    public LobbyHub(IConfiguration configuration)
    {
        var uri = $"{configuration.GetSection("ceres")["server-address"]}/{configuration.GetSection("ceres")["lobby-hub"]}";
        LobbyHubConnection = new HubConnectionBuilder() 
            .WithUrl(uri)
            .AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            })
            .WithAutomaticReconnect()
            .Build();
        LobbyHubConnection.Closed += NotifyUserOfServerDisconnected;
        LobbyHubConnection.Reconnecting += NotifyUserOfReconnecting;
        LobbyHubConnection.Reconnected += NotifyUserOfReconnected;
    }

    public async Task<bool> ConnectWithRetryAsync( CancellationToken token){
        // Keep trying to until we can start or the token is canceled.
        while (true)
        {
            try
            {
                Console.WriteLine($"Lobby Connection - trying to connect..");
                if (LobbyHubConnection != null)
                    await LobbyHubConnection.StartAsync(token);
                Console.WriteLine($"Lobby Connection - connected.");
                OnHubStataHasChanged?.Invoke();
                return true;
            }
            catch when (token.IsCancellationRequested)
            {
                return false;
            }
            catch
            {
                // Failed to connect, trying again in 5000 ms.
                await Task.Delay(5000, token);
            } 
        }
    }

    private Task NotifyUserOfReconnected(string? connectionId)
    {
        Console.WriteLine($"Lobby Connection successfully reconnected. The ConnectionId is now: {connectionId}");

        OnHubStataHasChanged?.Invoke();
        return Task.CompletedTask;
    }

    private async Task NotifyUserOfServerDisconnected(Exception? e)
    {
        Console.WriteLine(e != null ? $"Lobby Connection: Connection to server closed. Error: {e.Message}" : "Lobby Connection closed.");
        OnHubStataHasChanged?.Invoke();
        await ConnectWithRetryAsync(CancellationToken.None);
    }
    
    private Task NotifyUserOfReconnecting(Exception? e)
    {
        Console.WriteLine($"Lobby Connection started to reconnect due to an error: {e!.Message}");
        OnHubStataHasChanged?.Invoke();
        return Task.CompletedTask;
    }

    
}