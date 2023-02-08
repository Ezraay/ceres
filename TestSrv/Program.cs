using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public interface IServerAction
{
    void Apply();
}

public class Card
{
    public int Id { get; set; }
    public string Rank { get; set; }
    public string Suit { get; set; }
    public string Description { get; set; }
}

public class DrawCardAction : IServerAction
{
    public readonly Card Card;

    public DrawCardAction(Card card)
    {
        Card = card;
    }

    public void Apply()
    {
        Console.WriteLine($"Applying draw card action: {Card.Id}");
    }
}

public class PlayCardAction : IServerAction
{
    public readonly Card Card;

    public PlayCardAction(Card card)
    {
        Card = card;
    }

    public void Apply()
    {
        Console.WriteLine($"Applying play card action: {Card.Id}");
    }
}

public class GameHub : Hub
{
    public async Task SendAction(IServerAction action)
    {
        await Clients.All.SendAsync("ReceiveAction", action);
    }
}


class Program{

    static async Task Main(string[] args){
            
        var builder = WebApplication.CreateBuilder(args)
                .UseStartup<Startup>()
                .Build();

            // host.MapHub<GameHub>("/gameHub");

            host.Run();


        var drawCardAction = new DrawCardAction(new Card { Id = 100, Rank = "Ace", Suit = "Spades" });
        var playCardAction = new PlayCardAction(new Card { Id = 200, Rank = "King", Suit = "Hearts" });

            var hubContext = host.Services.GetRequiredService<IHubContext<GameHub>>();
            while (true)
            {
                Console.WriteLine("Enter 1 to send DrawCardAction, 2 to send PlayCardAction, or q to exit");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }

                if (input == "1")
                {
                    await hubContext.Clients.All.SendAsync("ReceiveAction", drawCardAction);
                }
                else if (input == "2")
                {
                    await hubContext.Clients.All.SendAsync("ReceiveAction", playCardAction);
                }
                else
                {
                    Console.WriteLine("Invalid input, try again");
                }
            }

    }

}

public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {

            // app.MapHub<LobbyHub>("/LobbyHub");
            // app.MapHub<GameHub>("/gameHub");
        }
    }