using Ceres.Core.BattleSystem;
using UnityEngine;

namespace Ceres.Client.BattleSystem
{
    // [CreateAssetMenu(menuName = "Create BattleManager", fileName = "BattleManager", order = 0)]
    public static class BattleManager
    {
        public static ClientBattle Battle { get; private set; }
        public static ICardDatabase CardDatabase { get; }

        static BattleManager()
        {
            string cardDataPath = "Data/Cards";
            TextAsset text = Resources.Load<TextAsset>(cardDataPath);
            CardDatabase = new CSVCardDatabase(text.text.Trim(), true);
        }

        public static void StartBattle(bool myTurn)
        {
            Debug.Log("Starting battle");
            AllyPlayer ally = new AllyPlayer();
            OpponentPlayer opponent = new OpponentPlayer();
            Battle = new ClientBattle(ally, opponent, myTurn);
        }

        // TODO: Send to client
        public static void Apply(IServerAction action)
        {
            Debug.Log("Got action from server: " + action);
            Battle.Apply(action);
        }

        public static void Execute(IClientCommand command)
        {
            if (command.CanExecute(Battle))
                // TODO: Send to server
                // NetworkManager.DoSomething();
                Debug.Log("Sending command to server: " + command);
        }
    }
}