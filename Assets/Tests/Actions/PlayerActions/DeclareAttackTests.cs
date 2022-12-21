using CardGame;
using NUnit.Framework;
using Tests.Slots;

namespace Tests.Actions.PlayerActions
{
    public class DeclareAttackTests
    {
        private TestCardData testCard = new TestCardData();
        
        [Test]
        public void CanExecuteTrue()
        {
            IPlayer player1 = new Player(null, new Card(testCard));
            IPlayer player2 = new Player(null, new Card(testCard));
            Battle battle = new Battle(player1, player2);

            battle.ExecuteImmediately(new SetPhase(BattlePhase.Attack));
            
            Assert.IsTrue(new DeclareAttack(player1.Champion).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new DeclareAttack(null).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new DeclareAttack(player2.Champion).CanExecute(battle, battle.Player2));
            Assert.IsFalse(new DeclareAttack(null).CanExecute(battle, battle.Player2));
        }
        
        [Test]
        public void CantExecuteOutsidePhase()
        {
            IPlayer player1 = new Player(null, new Card(testCard));
            IPlayer player2 = new Player(null, new Card(testCard));
            Battle battle = new Battle(player1, player2);
            
            Assert.IsFalse(new DeclareAttack(player1.Champion).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new DeclareAttack(null).CanExecute(battle, battle.Player1));
            Assert.IsFalse(new DeclareAttack(player2.Champion).CanExecute(battle, battle.Player2));
            Assert.IsFalse(new DeclareAttack(null).CanExecute(battle, battle.Player2));
        }

        [Test]
        public void Execute()
        {
            IPlayer player = new Player(null, new Card(testCard));
            Battle battle = TestBattle.CreateTestBattle(player);
            
            battle.ExecuteImmediately(new SetPhase(BattlePhase.Attack));
            battle.ExecuteImmediately(new DeclareAttack(battle.Player1.Champion));
            
            Assert.AreEqual(battle.CombatManager.Attacker, battle.Player1.Champion);
        }
    }
}