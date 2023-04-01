using System;
using CardGame.BattleDisplay.Networking;
using CardGame.Networking;
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
        public ClientBattle Battle { get; private set; }
        private ICommandProcessor commandProcessor;
        public ICardDatabase CardDatabase { get; private set; }
        public IDeck Deck { get; private set; }
        public IPlayer MyPlayer => commandProcessor.MyPlayer;

        [HideInInspector] public bool IsStarted = false;
        public event Action<IServerAction> OnAction;
        public event Action<BattleStartConditions> OnStartBattle;


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
            commandProcessor.OnStartBattle += battleStartConditions =>
            {
                Battle = battleStartConditions.ClientBattle;
                OnStartBattle?.Invoke(battleStartConditions);
            };
            commandProcessor.Start();
            IsStarted = true;
        }

        public void StartSinglePlayer()
        {
            Logger.Log("Starting single-player battle");
            bool myTurn = Random.Range(0f, 1f) < 0.5f; // In this case, local player is player 1
            ServerBattleStartConfig config = new ServerBattleStartConfig(Deck, Deck, myTurn);
            SinglePlayerProcessor singlePlayerProcessor = new SinglePlayerProcessor(config);
            commandProcessor = singlePlayerProcessor;
            commandProcessor.OnServerAction += action => OnAction?.Invoke(action);
            commandProcessor.OnStartBattle += conditions =>
            {
                Battle = conditions.ClientBattle;
                OnStartBattle?.Invoke(conditions);
            };
            commandProcessor.Start();
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