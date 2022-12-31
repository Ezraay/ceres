using System;
using System.Linq;
using Ceres.Core.BattleSystem.Battles;
using NUnit.Framework;

namespace Tests
{
    public class BattlePhaseManagerTests
    {
        [Test]
        public void AdvancePhase()
        {
            BattlePhaseManager manager = new BattlePhaseManager();
            BattlePhase phase = manager.Phase;
            manager.Advance();
            Assert.IsTrue(manager.Phase == phase + 1);
        }

        [Test]
        public void AdvanceLastPhase()
        {
            BattlePhaseManager manager = new BattlePhaseManager();
            manager.Set(manager.LastPhase);
            bool phaseEvent = false;
            manager.OnTurnEnd += () => phaseEvent = true;
            manager.Advance();
            Assert.IsTrue(phaseEvent);
            Assert.AreEqual(manager.Phase, manager.FirstPhase);
        }

        [Test]
        public void InitialiseFirstPhase()
        {
            BattlePhaseManager manager = new BattlePhaseManager();
            Assert.AreEqual(manager.Phase, manager.FirstPhase);
        }

        [Test]
        public void FirstPhase()
        {
            BattlePhase phase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Min();
            BattlePhaseManager manager = new BattlePhaseManager();
            Assert.AreEqual(manager.FirstPhase, phase);
        }
        
        [Test]
        public void LastPhase()
        {
            BattlePhase phase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Max();
            BattlePhaseManager manager = new BattlePhaseManager();
            Assert.AreEqual(manager.LastPhase, phase);
        }

        [Test]
        public void OnPhaseEnter()
        {
            BattlePhaseManager manager = new BattlePhaseManager();
            BattlePhase oldPhase = manager.Phase;
            BattlePhase phase = manager.LastPhase;
            manager.OnPhaseEnter += newPhase => phase = newPhase;
            manager.Advance(); 
            Assert.AreEqual(phase, oldPhase + 1);
        }

        [Test]
        public void OnPhaseExit()
        {
            BattlePhaseManager manager = new BattlePhaseManager();
            BattlePhase oldPhase = manager.Phase;
            BattlePhase phase = manager.LastPhase;
            manager.OnPhaseExit += newPhase => phase = newPhase;
            manager.Advance(); 
            Assert.AreEqual(phase, oldPhase);
        }
    }
}