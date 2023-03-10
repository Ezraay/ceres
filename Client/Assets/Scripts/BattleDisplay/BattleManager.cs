using System;
using CardGame.BattleDisplay.Networking;
using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;
using Random = UnityEngine.Random;

namespace Ceres.Client.BattleSystem
{
    // [CreateAssetMenu(menuName = "Create BattleManager", fileName = "BattleManager", order = 0)]
    public class BattleManager : MonoBehaviour
    {
        public ClientBattle Battle => commandProcessor?.ClientBattle; 
        private ICommandProcessor commandProcessor;
        public ICardDatabase CardDatabase { get; private set; }
        public IDeck Deck { get; private set; }
        [HideInInspector] public bool IsStarted = false;
        public event Action<IServerAction> OnAction;
        public event Action<ClientBattle> OnStartBattle;


        [Inject]
        public void Construct(ICardDatabase cardDatabase, IDeck testingDeck)
        {
            CardDatabase = cardDatabase;
            Deck = testingDeck;
        }

        public void StartMultiplayer(NetworkManager networkManager)
        {
            Logger.Log("Starting multiplayer battle");
            commandProcessor = new NetworkedProcessor(networkManager);
            commandProcessor.OnServerAction += action => OnAction?.Invoke(action);
            commandProcessor.OnStartBattle += battle => OnStartBattle?.Invoke(battle);
            IsStarted = true;
        }

        public void StartSinglePlayer()
        {
            Logger.Log("Starting single-player battle");
            bool myTurn = Random.Range(0f, 1f) < 0.5f; // In this case, local player is player 1
            ServerBattleStartConfig config = new ServerBattleStartConfig(Deck, Deck, myTurn);
            commandProcessor = new SingleplayerProcessor(config);
            commandProcessor.OnServerAction += action => OnAction?.Invoke(action);
            OnStartBattle?.Invoke(commandProcessor.ClientBattle);
            IsStarted = true;
        }

        public void Execute(IClientCommand command)
        {
            Logger.Log("Executing command: " + command);
            commandProcessor.ProcessCommand(command);
        }

        public void FakeAction(IServerAction action)
        {
            OnAction?.Invoke(action);
        }
    }
}