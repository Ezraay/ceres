﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Ceres.Core.Enums;
using Logger = Ceres.Client.Utility.Logger;
using UnityEngine;

namespace Ceres.Client
{
    public static class NetworkManager
    {
        private static HubConnection LobbyConnection;
        private static HubConnection GameConnection;
        public static event Action OnConnected; // TODO: Call this when connected
        public static event Action OnJoinGame;

        private static Guid UserId;
        private static Guid GameId;




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

            LobbyConnection.On<string, string>("GoToGame", (gameId, userId) => { MainThreadManager.Execute(OnGoToGame);
                Guid.TryParse(userId, out UserId);
                Guid.TryParse(gameId, out GameId);
            });

            if (LobbyConnection.State == HubConnectionState.Connected) OnConnected?.Invoke();
        }

        public static async Task JoinQueue()
        {
            string userName = "Unity";
            await LobbyConnection.SendAsync("UserIsReadyToPlay", userName, true);
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
                Logger.Log(res);

                OnJoinGame?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.Log("Error connecting to GameHub : " + ex.Message);
                Debug.Log("StackTrace: " + ex.StackTrace);
            }
            
        }
    }
}