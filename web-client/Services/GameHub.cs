
namespace webclient.Services;

public class GameHub : SignalRHub
{
    public GameHub(IConfiguration configuration): base(configuration,"game-hub"){}

}