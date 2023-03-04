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
    public class BattleSystemManager : MonoBehaviour
    {
        // public static ClientBattle Battle { get; private set; }
        private IBattleManager battleManager;
        public ICardDatabase CardDatabase { get; private set; }
        public IDeck Deck { get; private set; }
        [FormerlySerializedAs("Started")] public bool IsStarted = false;
        public static event Action<IServerAction> OnAction;


        [Inject]
        public void Construct(ICardDatabase cardDatabase, IDeck testingDeck)
        {
            CardDatabase = cardDatabase;
            Deck = testingDeck;
        }

        public void StartMultiplayer(NetworkManager networkManager)
        {
            Logger.Log("Starting multiplayer battle");
            battleManager = new NetworkedBattleManager(networkManager);
            battleManager.OnServerAction += action => OnAction?.Invoke(action);
            IsStarted = true;
        }

        public void StartSinglePlayer()
        {
            Logger.Log("Starting singleplayer battle");
            bool myTurn = Random.Range(0f, 1f) < 0.5f; // In this case, local player is player 1
            ServerBattleStartConfig config = new ServerBattleStartConfig(Deck, Deck, myTurn);
            battleManager = new LocalBattleManager(config);
            battleManager.OnServerAction += action => OnAction?.Invoke(action);
            IsStarted = true;
        }

        public void Execute(IClientCommand command)
        {
            Logger.Log("Executing command: " + command);
            battleManager.ProcessCommand(command);
        }
    }
}