using Microsoft.AspNetCore.SignalR.Client;


namespace webclient.Services;

public class LobbyHub
{
    public LobbyHub(IConfiguration configuration)
    {
        var uri = $"{configuration.GetSection("ceres")["server-address"]}/{configuration.GetSection("ceres")["lobby-hub"]}";
        LobbyHubConnection = new HubConnectionBuilder() 
            .WithUrl(uri)
            .WithAutomaticReconnect()
            .Build();
    }

    public HubConnection LobbyHubConnection { get; }

}