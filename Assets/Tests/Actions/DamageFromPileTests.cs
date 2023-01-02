﻿using System.Collections.Generic;
using Ceres.Core.BattleSystem.Actions;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;
using NUnit.Framework;
using Tests.Slots;

namespace Tests.Actions
{
    public class DamageFromPileTests
    {
        [Test]
        public void CantExecuteWhenNoCardsInPile()
        {
            IPlayer player = new Player(new List<ICard>(), null);
            DamageFromPile command = new DamageFromPile();

            Assert.IsFalse(command.CanExecute(null, player));
        }

        [Test]
        public void CanExecute()
        {
            ICard card = new Card(new TestCardData());
            IPlayer player = new Player(new List<ICard> {card}, null);
            DamageFromPile command = new DamageFromPile();
            Assert.IsTrue(command.CanExecute(null, player));
        }

        [Test]
        public void ShouldAddDamage()
        {
            ICard card = new Card(new TestCardData());
            List<ICard> pile = new List<ICard> {card};
            IPlayer player = new Player(pile, null);

            DamageFromPile command = new DamageFromPile();
            command.Execute(null, player);

            Assert.IsTrue(player.Damage.Count > 0);
            Assert.AreEqual(0, player.Pile.Count);
        }
    }
}