using System.Collections.Generic;
using Ceres.Core.BattleSystem.Actions;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;
using Ceres.Core.BattleSystem.Slots;
using NUnit.Framework;
using Tests.Slots;

namespace Tests.Actions
{
    public class AlertTests
    {
        [Test]
        public void CantAlertNull()
        {
            IPlayer player = new Player();
            Alert command = new Alert(-1, -1);
            Assert.IsFalse(command.CanExecute(null, player));
        }

        [Test]
        public void CantAlertEmptySlot()
        {
            IPlayer player = new Player();
            Alert command = new Alert(player.Champion.x, player.Champion.y);
            Assert.IsNull(player.Champion.Card);
            Assert.IsFalse(command.CanExecute(null, player));
        }
        
        [Test]
        public void AlertShouldAffectSlot()
        {
            ICard card = new Card(new TestCardData());
            IPlayer player = new Player(new List<ICard>(), card);
            player.Champion.Exhaust();

            Alert command = new Alert(player.Champion.x, player.Champion.y);
            command.Execute(null, player);
            
            Assert.IsFalse(player.Champion.Exhausted);
        }
    }
}