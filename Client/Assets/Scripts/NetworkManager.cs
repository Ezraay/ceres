using System;
using System.Threading.Tasks;
using Ceres.Core.BattleSystem;
using Ceres.Core.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace Ceres.Client
{
    public static class NetworkManager
    {
        private static HubConnection LobbyConnection;
        private static HubConnection GameConnection;

        private static Guid UserId;
        private static Guid GameId;
        public static event Action OnConnected; // TODO: Call this when connected
        public static event Action<bool> OnJoinGame;
        public static event Action<IServerAction> OnBattleAction;


        public static async void Connect()
        {
            LobbyConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5146/LobbyHub")
                .Build();

            try
            {
                await LobbyConnection.StartAsync();
                Debug.Log("SignalRConnector connected");
            }
            catch (Exception ex)
            {
                Debug.Log("Error connecting to the SignalR server: " + ex.Message);
                Debug.Log("StackTrace: " + ex.StackTrace);
            }

            LobbyConnection.On<string, string>("GoToGame", (gameId, userId) =>
            {
                MainThreadManager.Execute(OnGoToGame);
                Guid.TryParse(userId, out UserId);
                Guid.TryParse(gameId, out GameId);
            });

            GameConnection.On<IServerAction>("ServerAction", action =>
            {
                MainThreadManager.Execute(() =>
                {
                    OnBattleAction?.Invoke(action);
                });
            });

            if (LobbyConnection.State == HubConnectionState.Connected) OnConnected?.Invoke();
        }

        public static async Task JoinQueue()
        {
            string userName = "Unity";
            await LobbyConnection.SendAsync("UserIsReadyToPlay", userName, true);
        }

        public static void SendCommand(IClientCommand command)
        {
            GameConnection.SendAsync("PlayerSentCommand", GameId, UserId, command);
        }

        private static async void OnGoToGame()
        {
            GameConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5146/GameHub")
                .Build();

            try
            {
                await GameConnection.StartAsync();
                Debug.Log("SignalRConnector - connected to GameHub");

                string res = await GameConnection.InvokeAsync<string>("JoinGame", GameId, UserId);
                if (res == JoinGameResults.JoinedAsPlayer1)
                    Logger.Log("I am Player1");
                if (res == JoinGameResults.JoinedAsPlayer2)
                    Logger.Log("I am Player2");

                OnJoinGame?.Invoke(res == JoinGameResults.JoinedAsPlayer1);
            }
            catch (Exception ex)
            {
                Debug.Log("Error connecting to GameHub : " + ex.Message);
                Debug.Log("StackTrace: " + ex.StackTrace);
            }
        }
    }
}