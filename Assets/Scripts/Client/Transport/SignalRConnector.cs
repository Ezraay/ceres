using System;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Connections;

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
            // await connection.InvokeAsync("ClientConnected");
        }
        catch (Exception ex)
        {
            Debug.Log("Error connecting to the SignalR server: " +ex.Message);
            Debug.Log("StackTrace: " +ex.StackTrace);
        }
    }


}