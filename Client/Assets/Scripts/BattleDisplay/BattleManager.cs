﻿using Ceres.Core.BattleSystem;
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
            NetworkManager.OnBattleAction += Apply;
        }

        public static void StartBattle(bool myTurn)
        {
            AllyPlayer ally = new AllyPlayer();
            OpponentPlayer opponent = new OpponentPlayer();
            Battle = new ClientBattle(ally, opponent, myTurn);
        }

        public static void Apply(IServerAction action)
        {
            
            Battle.Apply(action);
        }

        public static void Execute(IClientCommand command)
        {
            if (command.CanExecute(Battle))
            {
                NetworkManager.SendCommand(command);
            }
        }
    }
}