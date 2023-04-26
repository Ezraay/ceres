
namespace webclient.Services;

public class LobbyHub : SignalRHub
{
    public LobbyHub(IConfiguration configuration) : base(configuration, "lobby-hub"){}

}