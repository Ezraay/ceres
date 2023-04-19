using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem.Battles;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{
	public class Battle
	{
		public readonly CombatManager CombatManager;
		public readonly PhaseManager PhaseManager;
		public readonly TeamManager TeamManager;

		public Battle(TeamManager teamManager, PhaseManager phaseManager)
		{
			this.CombatManager = new CombatManager();
			this.PhaseManager = new PhaseManager();
			this.TeamManager = teamManager;

			this.PhaseManager.OnPhaseEnter += phase =>
			{
				switch (phase)
				{
					case BattlePhase.Stand:
						this.CombatManager.Reset();
						break;
				}
			};
		}

		public virtual void StartGame(List<IPlayer> playerOrder)
		{
			this.PhaseManager.SetPlayers(playerOrder);
		}

		public bool HasPriority(IPlayer player)
		{
			return true; // TODO
		}
	}
}