using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
public class SignalRConnector
{

    private HubConnection connection;
    public async Task InitAsync()
    {
        connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5146/Lobby")
                .Build();
        // connection.On<string, string>("ReceiveMessage", (user, message) =>
        // {
        //     OnMessageReceived?.Invoke(new Message
        //     {
        //         UserName = user,
        //         Text = message,
        //     });
        // });
        await StartConnectionAsync();
    }

    private async Task StartConnectionAsync()
    {
        try
        {
            await connection.StartAsync();
            UnityEngine.Debug.Log("SignalRConnector connected");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error {ex.Message}");
        }
    }
}