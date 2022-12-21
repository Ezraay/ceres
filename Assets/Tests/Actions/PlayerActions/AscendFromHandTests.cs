﻿using CardGame;
using NUnit.Framework;
using Tests.Slots;

namespace Tests.Actions.PlayerActions
{
    public class AscendFromHandTests
    {
        private TestCardData first;
        private TestCardData second;
        private TestCardData third;

        [SetUp]
        public void Setup()
        {
            first = new TestCardData();
            second = new TestCardData
            {
                Tier = 3
            };
            third = new TestCardData();
        }

        [Test]
        public void CanExecuteTrue()
        {
            IPlayer player1 = new Player(null, new Card(first));
            IPlayer player2 = new Player(null, new Card(first));
            Battle battle = new Battle(player1, player2);

            Card card1 = new Card(first);
            Card card2 = new Card(second);
            Card card3 = new Card(third);
            player1.Hand.AddCard(card1);
            player1.Hand.AddCard(card2);
            player2.Hand.AddCard(card1);
            player2.Hand.AddCard(card2);

            battle.BattlePhaseManager.Set(BattlePhase.Ascend);

            Assert.IsTrue(new AscendFromHand(card1).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new AscendFromHand(card2).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new AscendFromHand(card3).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new AscendFromHand(card1).CanExecute(battle, battle.Player2));
            Assert.IsFalse(new AscendFromHand(card2).CanExecute(battle, battle.Player2));
            Assert.IsFalse(new AscendFromHand(card3).CanExecute(battle, battle.Player2));
        }

        [Test]
        public void Execute()
        {
            IPlayer player = new Player(null, new Card(first));
            Battle battle = new Battle(player, new NullPlayer());
            Card card = new Card(first);
            player.Hand.AddCard(card);

            battle.ExecuteImmediately(new SetPhase(BattlePhase.Ascend));
            battle.ExecuteImmediately(new AscendFromHand(card));
            Assert.AreEqual(card, player.Champion.Card);
        }
    }
}