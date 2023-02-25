using System;
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
    public class NetworkManager : MonoBehaviour
    {
        private HubConnection LobbyConnection;
        private HubConnection GameConnection;

        private Guid UserId;
        private Guid GameId;

        private MainThreadManager mainThreadManager;
        public bool Connected { get; private set; }
        public event Action OnConnected; // TODO: Call this when connected
        public event Action<ClientBattleStartConfig> OnStartGame;
        public event Action<IServerAction> OnBattleAction;

        [Inject]
        public void Construct(MainThreadManager mainThreadManager)
        {
            this.mainThreadManager = mainThreadManager;
        }


        public async void Connect()
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


            if (LobbyConnection.State == HubConnectionState.Connected)
            {
                OnConnected?.Invoke();
                Connected = true;
            }
        }

        public void JoinQueue()
        {
            string userName = "Unity";
            LobbyConnection.SendAsync("UserIsReadyToPlay", userName, true);
        }

        public void SendCommand(IClientCommand command)
        {
            GameConnection.SendAsync("PlayerSentCommand", GameId, UserId, command);
        }

        private async void OnGoToGame()
        {
            GameConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5146/GameHub")
                .AddNewtonsoftJsonProtocol(options =>
                {
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

                ClientBattleStartConfig config = new ClientBattleStartConfig(new CardData("archer", "Archer", 1, 5, 5),
                    res == JoinGameResults.JoinedAsPlayer1, 50, 50); // TODO: Get this from server
                OnStartGame?.Invoke(config);

                GameConnection.On<IServerAction>("ServerAction", action =>
                {
                    mainThreadManager.Execute(() =>
                    {
                        Logger.Log("Got action: " + action);
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