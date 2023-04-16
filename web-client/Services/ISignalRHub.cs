using Microsoft.AspNetCore.SignalR.Client;

namespace webclient.Services;

public interface ISignalRHub
{
    HubConnection LobbyHubConnection { get; }
    Task<bool> ConnectWithRetryAsync(CancellationToken token);
    event Action? OnHubStataHasChanged;
}