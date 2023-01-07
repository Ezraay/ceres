using Ceres.Core.BattleSystem;
using UnityEngine;

namespace Ceres.Client.BattleSystem
{
    [CreateAssetMenu(menuName = "Create BattleManager", fileName = "BattleManager", order = 0)]
    public class BattleManager : ScriptableObject
    {
        public ClientBattle Battle { get; private set; }
        public ICardDatabase CardDatabase { get; private set; }

        private void OnEnable()
        {
            string cardDataPath = "Data/Cards";
            TextAsset text = Resources.Load<TextAsset>(cardDataPath);
            CardDatabase = new CSVCardDatabase(text.text.Trim(), true);

            CreateBattle();
        }

        public void CreateBattle()
        {
            AllyPlayer ally = new AllyPlayer();
            OpponentPlayer opponent = new OpponentPlayer();
            Battle = new ClientBattle(ally, opponent);
        }

        // TODO: Send to client
        public void Apply(IServerAction action)
        {
            Battle.Apply(action);
        }

        public void Execute(IClientCommand command)
        {
            if (command.CanExecute(Battle))
                // TODO: Send to server
                Debug.Log("Sending command to server: " + command);
        }
    }
}