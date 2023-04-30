using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem.Battles;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{
	public abstract class Battle
	{
		public readonly CombatManager CombatManager;
		public readonly PhaseManager PhaseManager;
		public readonly IPlayer Player1;
		public readonly IPlayer Player2;

		protected Battle(PhaseManager phaseManager, IPlayer player1, IPlayer player2)
		{
			this.CombatManager = new CombatManager();
			this.PhaseManager = phaseManager;
			this.Player1 = player1;
			this.Player2 = player2;

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

		public IPlayer GetEnemy(IPlayer player)
		{
			return player == this.Player1 ? this.Player2 : this.Player1;
		}

		public virtual void StartGame(List<IPlayer> playerOrder)
		{
			this.PhaseManager.SetPlayers(playerOrder);
		}
	}
}