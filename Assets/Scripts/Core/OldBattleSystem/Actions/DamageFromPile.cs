﻿using Ceres.Core.OldBattleSystem.Battles;
using Ceres.Core.OldBattleSystem.Players;

namespace Ceres.Core.OldBattleSystem.Actions
{
    public class DamageFromPile : IAction
    {
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return player.Pile.Count > 0;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            player.Damage.AddCard(player.Pile.PopCard());
        }
    }
}