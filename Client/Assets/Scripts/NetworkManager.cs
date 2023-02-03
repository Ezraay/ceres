using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Ceres.Client
{
    public static class NetworkManager
    {
        public static HubConnection Connection;
        public static event Action OnConnected; // TODO: Call this when connected
        public static event Action OnJoinGame;


        public static async void Connect()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5146/LobbyHub")
                .Build();

            try
            {
                await Connection.StartAsync();
                Debug.Log("SignalRConnector connected");
            }
            catch (Exception ex)
            {
                Debug.Log("Error connecting to the SignalR server: " + ex.Message);
                Debug.Log("StackTrace: " + ex.StackTrace);
            }

            Connection.On<string>("GoToGame", gameId => { MainThreadManager.Execute(OnGoToGame); });

            if (Connection.State == HubConnectionState.Connected) OnConnected?.Invoke();
        }

        public static async Task JoinQueue()
        {
            string userName = "Unity";
            await Connection.SendAsync("UserIsReadyToPlay", userName, true);
        }

        private static void OnGoToGame()
        {
            OnJoinGame?.Invoke();
        }
    }
}