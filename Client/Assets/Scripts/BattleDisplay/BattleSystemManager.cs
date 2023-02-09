using System;
using CardGame.BattleDisplay.Networking;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;
using Random = UnityEngine.Random;

namespace Ceres.Client.BattleSystem
{
    // [CreateAssetMenu(menuName = "Create BattleManager", fileName = "BattleManager", order = 0)]
    public static class BattleSystemManager
    {
        // public static ClientBattle Battle { get; private set; }
        private static IBattleManager battleManager;
        public static ICardDatabase CardDatabase { get; }
        public static bool Started => battleManager != null;
        public static event Action<IServerAction> OnAction;

        static BattleSystemManager()
        {
            string cardDataPath = "Data/Cards";
            TextAsset text = Resources.Load<TextAsset>(cardDataPath);
            CardDatabase = new CSVCardDatabase(text.text.Trim(), true);
            NetworkManager.OnBattleAction += Apply;
            OnAction += action => Logger.Log("On action called");
        }

        public static void StartMultiplayer()
        {
            Logger.Log("Starting multiplayer battle");
            battleManager = new NetworkedBattleManager();
            battleManager.OnServerAction += OnAction;
        }

        public static void StartSinglePlayer()
        {
            Logger.Log("Starting singleplayer battle");
            string cardDataPath = "Data/Testing Deck";
            TextAsset text = Resources.Load<TextAsset>(cardDataPath);
            IDeck baseDeck = new CSVDeck(CardDatabase, text.text.Trim());
            bool myTurn = Random.Range(0f, 1f) < 0.5f; // In this case, local player is player 1
            ServerBattleStartConfig config = new ServerBattleStartConfig(baseDeck, baseDeck, myTurn);
            battleManager = new LocalBattleManager(config);
            battleManager.OnServerAction += OnAction;
        }

        public static void StartBattle(bool myTurn)
        {
            // AllyPlayer ally = new AllyPlayer();
            // OpponentPlayer opponent = new OpponentPlayer();
            // Battle = new ClientBattle(ally, opponent, myTurn);
        }

        public static void Apply(IServerAction action)
        {
            
            // Battle.Apply(action);
        }

        public static void Execute(IClientCommand command)
        {
            Logger.Log("Executing command: " + command);
            battleManager.ProcessCommand(command);
            // if (command.CanExecute(Battle))
            // {
            //     NetworkManager.SendCommand(command);
            // }
        }
    }
}