using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Players;

namespace Ceres.Core.BattleSystem.Actions
{
    public class DrawFromPile : IAction
    {
        public bool CanExecute(Battle battle, IPlayer player)
        {
            return player.Pile.Cards.Count > 0;
        }

        public void Execute(Battle battle, IPlayer player)
        {
            player.Hand.AddCard(player.Pile.PopCard());
        }
    }
}