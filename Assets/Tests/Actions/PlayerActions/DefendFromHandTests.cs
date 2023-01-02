using Ceres.Core.BattleSystem.Actions;
using Ceres.Core.BattleSystem.Actions.PlayerActions;
using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;
using NUnit.Framework;
using Tests.Slots;
using UnityEngine;

namespace Tests.Actions.PlayerActions
{
    public class DefendFromHandTests
    {
        private readonly TestCardData testCard = new TestCardData();


        [Test]
        public void CantDefendWhenAttacking()
        {
            IPlayer player = new Player();
            ICard card = new Card(testCard);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(player);
            IAction command = new DefendFromHand(card);
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendOutsidePhase()
        {
            IPlayer player = new Player();
            ICard card = new Card(testCard);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(new NullPlayer(), player);
            IAction command = new DefendFromHand(card);
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendHigherTier()
        {
            IPlayer player = new Player(null, new Card(testCard));
            ICardData cardData = new TestCardData
            {
                Tier = 3
            };
            ICard card = new Card(cardData);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(new NullPlayer(), player);
            IAction command = new DefendFromHand(card);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Defend));
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendWithNull()
        {
            IPlayer player = new Player();
            Battle battle = TestBattle.CreateTestBattle(new NullPlayer(), player);
            IAction command = new DefendFromHand(null);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Defend));
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CantDefendWithCardNotInHand()
        {
            IPlayer player = new Player();
            ICard card = new Card(testCard);
            Battle battle = TestBattle.CreateTestBattle(new NullPlayer(), player);
            IAction command = new DefendFromHand(card);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Defend));
            Assert.IsFalse(command.CanExecute(battle, player));
        }

        [Test]
        public void CanDefend()
        {
            IPlayer player = new Player(null, new Card(testCard));
            ICard card = new Card(testCard);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(new NullPlayer(), player);
            DefendFromHand command = new DefendFromHand(card);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Defend));
            Assert.IsTrue(command.CanExecute(battle, player));
        }

        [Test]
        public void Execute()
        {
            IPlayer player = new Player(null, new Card(testCard));
            ICard card = new Card(testCard);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(new Player(), player);
            
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Defend));
            battle.ExecuteImmediately(new DefendFromHand(card), player);

            Assert.Contains(card, battle.CombatManager.Defenders.Cards);
        }
    }
}