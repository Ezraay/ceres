using System;
using CardGame.BattleDisplay.Networking;
using CardGame.Networking;
using Ceres.Client.BattleSystem;
using Ceres.Client.Networking;
using Ceres.Core.BattleSystem;
using Ceres.Core.Entities;
using Ceres.Core.Networking.Messages;
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
        private SceneManager sceneManager;

        private Guid userId;
        public bool IsConnected { get; private set; }

        public event Action<BattleStartConditions> OnStartGame;
        public event Action<EndBattleReason> OnGameEnd;
        public event Action<IServerAction> OnBattleAction;

        [Inject]
        public void Construct(MainThreadManager mainThread, SignalRManager signalR, SceneManager scene)
        {
            mainThreadManager = mainThread;
            signalRManager = signalR;
            this.sceneManager = scene;
            
            
            signalRManager.OnConnected += Connected;
        }

        private void Connected()
        {
            signalRManager.On<GoToGameMessage>(signalRManager.LobbyHub, async message =>
            {
                userId = message.UserId;
                gameId = message.GameId;
                await signalRManager.ConnectToGameHub();

                mainThreadManager.Execute(() => 
                    OnGoToGame(message.ClientBattle, message.PlayerId));
            });

            signalRManager.On<GameEndedMessage>(signalRManager.GameHub, async message =>
            {
                await signalRManager.DisconnectFromGameHub();
                mainThreadManager.Execute(async () => 
                    this.OnGameEnd?.Invoke(message.Reason));
            });
            
            signalRManager.On<ServerActionMessage>(signalRManager.GameHub, message =>
            {
                mainThreadManager.Execute(() =>
                {
                    Logger.Log("Got action: " + message.Action);
                    OnBattleAction?.Invoke(message.Action);
                });
            });
            
            IsConnected = true;
            sceneManager.LoadScene(new MainMenuScene());
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

        private async void OnGoToGame(ClientBattle battle, Guid playerId)
        {
            BattleScene battleScene = new BattleScene(new NetworkedProcessor(this));
            battleScene.OnSceneLeave += LeaveGame;
            await sceneManager.LoadSceneAsync(battleScene);

            BattleStartConditions conditions = new BattleStartConditions()
            {
                ClientBattle = battle,
                PlayerId = playerId
            };
            OnStartGame?.Invoke(conditions);

            await signalRManager.GameHub.SendAsync("JoinGame",  gameId, userId);
        }

        private void LeaveGame()
        {
            // this.signalRManager.DisconnectGameHub();
        }
    }
}