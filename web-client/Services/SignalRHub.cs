using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace webclient.Services;

public class SignalRHub: ISignalRHub
{
    private readonly string hubAddress;
    public HubConnection HubConnection { get; }
    public event Action? OnHubStataHasChanged;
    public SignalRHub(IConfiguration configuration, string hubAddress)
    {
        this.hubAddress = hubAddress;
        var uri = $"{configuration.GetSection("ceres")["server-address"]}/{configuration.GetSection("ceres")[hubAddress]}";
        HubConnection = new HubConnectionBuilder() 
            .WithUrl(uri)
            .AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            })
            .WithAutomaticReconnect()
            .Build();
        HubConnection.Closed += NotifyUserOfServerDisconnected;
        HubConnection.Reconnecting += NotifyUserOfReconnecting;
        HubConnection.Reconnected += NotifyUserOfReconnected;
    }

    public async Task<bool> ConnectWithRetryAsync( CancellationToken token){
        // Keep trying to until we can start or the token is canceled.
        while (true)
        {
            try
            {
                Console.WriteLine($"{hubAddress} Connection - trying to connect..");
                if (HubConnection != null)
                    await HubConnection.StartAsync(token);
                Console.WriteLine($"{hubAddress} Connection - connected.");
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
        Console.WriteLine($"{hubAddress} Connection successfully reconnected. The ConnectionId is now: {connectionId}");

        OnHubStataHasChanged?.Invoke();
        return Task.CompletedTask;
    }

    private async Task NotifyUserOfServerDisconnected(Exception? e)
    {
        Console.WriteLine(e != null ? $"{hubAddress} Connection: Connection to server closed. Error: {e.Message}" : $"{hubAddress} Connection closed.");
        OnHubStataHasChanged?.Invoke();
        await ConnectWithRetryAsync(CancellationToken.None);
    }
    
    private Task NotifyUserOfReconnecting(Exception? e)
    {
        Console.WriteLine($"{hubAddress} Connection started to reconnect due to an error: {e!.Message}");
        OnHubStataHasChanged?.Invoke();
        return Task.CompletedTask;
    }

    
}