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
using Random = System.Random;

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
            
            
            signalRManager.OnLobbyConnected += LobbyConnected;
        }

        private async void OnGoToGameMessage(GoToGameMessage message)
        {
            userId = message.UserId;
            gameId = message.GameId;
            await signalRManager.ConnectToGameHubAsync();

            mainThreadManager.Execute(() => 
                OnGoToGame(message.ClientBattle, message.PlayerId));
        }

        private async void OnGameEndedMessage(GameEndedMessage message)
        {
            await signalRManager.DisconnectFromGameHubAsync();
            mainThreadManager.Execute(async () => 
                this.OnGameEnd?.Invoke(message.Reason));
        }

        private void OnServerActionMessage(ServerActionMessage message)
        {
            mainThreadManager.Execute(() =>
            {
                Logger.Log("Got action: " + message.Action);
                OnBattleAction?.Invoke(message.Action);
            });
        }

        private void LobbyConnected()
        {
            signalRManager.OnLobbyMessage<GoToGameMessage>(OnGoToGameMessage);
            signalRManager.OnGameMesage<GameEndedMessage>(OnGameEndedMessage);
            signalRManager.OnGameMesage<ServerActionMessage>(OnServerActionMessage);
            
            IsConnected = true;
            sceneManager.LoadScene(new MainMenuScene());
        }

        public void JoinQueue()
        {
            var userName = "Unity" + new Random().Next(42).ToString();
            var msg = new ClientReadyToPlayNetworkMessage() { UserName = userName, Ready = true };
            signalRManager.SendToLobbyAsync(msg);
        }

        public void SendCommand(IClientCommand command)
        {
            var msg = new ClientPlayerSentCommandMessage()
            {
                PlayerCommand = command, GameId = gameId, UserId = userId
            };
            signalRManager.SendToGameAsync(msg);
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

            var msg = new ClientJoinGameMessage() { GameId = gameId, UserId = userId };
            signalRManager.SendToGameAsync(msg);
        }

        private async void LeaveGame()
        {
            await signalRManager.DisconnectFromGameHubAsync();
        }
    }
}