using Ceres.Core.BattleSystem.Actions;
using Ceres.Core.BattleSystem.Actions.PlayerActions;
using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;
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
        public void CanExecute()
        {
            ICardData data = new TestCardData();
            Player player = new Player(null, new Card(data));
            ICard card = new Card(first);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(player);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Ascend));

            AscendFromHand command = new AscendFromHand(card.ID);
            command.Execute(battle, player);
            Assert.AreEqual(card, player.Champion.Card);
            Assert.IsFalse(player.Hand.Contains(card));
        }

        [Test]
        public void CantAscendOutsideTurn()
        {
            ICardData data = new TestCardData();
            Player player = new Player(null, new Card(data));
            ICard card = new Card(first);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(new NullPlayer(), player);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Ascend));

            AscendFromHand command = new AscendFromHand(card.ID);
            command.Execute(battle, player);
            Assert.AreEqual(card, player.Champion.Card);
            Assert.IsFalse(player.Hand.Contains(card));
        }

        [Test]
        public void CantAscendOutsidePhase()
        {
            ICardData data = new TestCardData();
            Player player = new Player(null, new Card(data));
            ICard card = new Card(first);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(player);

            AscendFromHand command = new AscendFromHand(card.ID);
            command.Execute(battle, player);
            Assert.AreEqual(card, player.Champion.Card);
            Assert.IsFalse(player.Hand.Contains(card));
        }

        [Test]
        public void CantAscendWithLowerTier()
        {
            ICardData data = new TestCardData();
            ICardData champion = new TestCardData
            {
                Tier = 2
            };
            Player player = new Player(null, new Card(champion));
            ICard card = new Card(first);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(player);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Ascend));

            AscendFromHand command = new AscendFromHand(card.ID);
            command.Execute(battle, player);
            Assert.AreEqual(card, player.Champion.Card);
            Assert.IsFalse(player.Hand.Contains(card));
        }

        [Test]
        public void CantAscendWithHigherTier()
        {
            ICardData data = new TestCardData
            {
                Tier = 3
            };
            ICardData champion = new TestCardData();
            Player player = new Player(null, new Card(champion));
            ICard card = new Card(first);
            player.Hand.AddCard(card);
            Battle battle = TestBattle.CreateTestBattle(player);
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Ascend));

            AscendFromHand command = new AscendFromHand(card.ID);
            command.Execute(battle, player);
            Assert.AreEqual(card, player.Champion.Card);
            Assert.IsFalse(player.Hand.Contains(card));
        }

        [Test]
        public void Execute()
        {
            IPlayer player = new Player(null, new Card(first));
            Battle battle = new Battle(player, new NullPlayer());
            Card card = new Card(first);
            player.Hand.AddCard(card);

            battle.ExecuteImmediately(new SetPhase(BattlePhase.Ascend));
            battle.ExecuteImmediately(new AscendFromHand(card.ID));
            Assert.AreEqual(card, player.Champion.Card);
        }
    }
}