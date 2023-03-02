using System;
using Ceres.Client.Networking;
using Ceres.Core.BattleSystem;
using Ceres.Core.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;

namespace Ceres.Client
{
    public class NetworkManager : MonoBehaviour
    {
        private Guid gameId;

        private MainThreadManager mainThreadManager;

        private SignalRManager signalRManager;

        private Guid userId;
        public bool IsConnected { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public event Action<ClientBattleStartConfig> OnStartGame;
        public event Action<IServerAction> OnBattleAction;

        [Inject]
        public void Construct(MainThreadManager mainThread, SignalRManager signalR)
        {
            mainThreadManager = mainThread;
            signalRManager = signalR;
            signalRManager.OnConnected += Connected;
        }

        private void Connected()
        {
            signalRManager.On<GoToGameMessage>(signalRManager.LobbyHub, async message =>
            {
                userId = message.UserId;
                gameId = message.GameId;
                await signalRManager.ConnectToGameHub();

                mainThreadManager.Execute(OnGoToGame);
            });

            IsConnected = true;
            SceneManager.LoadScene(GameScene.MainMenu);
        }

        public void JoinQueue()
        {
            string userName = "Unity";
            signalRManager.LobbyHub.SendAsync("UserIsReadyToPlay", userName, true);
        }

        public void SendCommand(IClientCommand command)
        {
            signalRManager.GameHub.SendAsync("PlayerSentCommand", gameId, userId, command);
        }

        private async void OnGoToGame()
        {
            SceneManager.LoadScene(GameScene.Battle);

            string res = await signalRManager.GameHub.InvokeAsync<string>("JoinGame", gameId, userId);
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

            signalRManager.On<ServerActionMessage>(signalRManager.GameHub, message =>
            {
                mainThreadManager.Execute(() =>
                {
                    Logger.Log("Got action: " + message.Action);
                    OnBattleAction?.Invoke(message.Action);
                });
            });
        }
    }
}