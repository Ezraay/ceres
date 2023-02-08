using System;
using System.Threading.Tasks;
using Ceres.Core.BattleSystem;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5001/gameHub")
            .Build();

        connection.On<IServerAction>("ReceiveAction", (action) =>
        {
            action.Apply(null);
        });

        await connection.StartAsync();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
