using System;
using System.Reflection;
using System.Threading.Tasks;
using Ceres.Core.BattleSystem;
using Ceres.Core.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;


namespace Ceres.Client
{
    public class NetworkManager
    {
        private static HubConnection LobbyConnection;
        private static HubConnection GameConnection;

        private static Guid UserId;
        private static Guid GameId;
        public static event Action OnConnected; // TODO: Call this when connected
        public static event Action<bool> OnJoinGame;
        public static event Action<IServerAction> OnBattleAction;

        private static MainThreadManager mainThreadManager;
        
        [Inject]
        public void Constructor(MainThreadManager mainThreadManager)
        {
            NetworkManager.mainThreadManager = mainThreadManager;
            Debug.Log(mainThreadManager);
        }


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
                mainThreadManager.Execute(OnGoToGame);
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

        public static void SendCommand(IClientCommand command)
        {
            GameConnection.SendAsync("PlayerSentCommand", GameId, UserId, command);
        }




        private static async void OnGoToGame()
        {
            GameConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5146/GameHub")
                .AddNewtonsoftJsonProtocol(options => {
                    options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.All;
                })
                .Build();



            try
            {
                await GameConnection.StartAsync();
                Debug.Log("SignalRConnector - connected to GameHub");

                string res = await GameConnection.InvokeAsync<string>("JoinGame", GameId, UserId);
                switch (res)
                {
                    case JoinGameResults.JoinedAsPlayer1:
                        Logger.Log("I am Player1");
                        break;
                    case JoinGameResults.JoinedAsPlayer2:
                        Logger.Log("I am Player2");
                        break;
                }

                OnJoinGame?.Invoke(res == JoinGameResults.JoinedAsPlayer1);

                GameConnection.On<IServerAction>("ServerAction", (action) =>
                {
                    mainThreadManager.Execute(() =>
                    {
                        Logger.Log("Got action: " + action);
                        // Logger.Log($"Action = {action} of a type = {action.GetType().FullName}");
                        // JsonConvert.DeserializeObject<type.typeof()>(action); 
                        OnBattleAction?.Invoke(action);
                    });
                });

                
            }
            catch (Exception ex)
            {
                Debug.Log("Error connecting to GameHub : " + ex.Message);
                Debug.Log("StackTrace: " + ex.StackTrace);
            }

        }
    }
}