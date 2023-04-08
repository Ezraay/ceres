using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem.Battles;

namespace Ceres.Core.BattleSystem
{
	public class Battle
	{
		public readonly CombatManager CombatManager;
		public readonly PhaseManager PhaseManager;
		public readonly TeamManager TeamManager;

		public Battle(TeamManager teamManager)
		{
			this.CombatManager = new CombatManager();
			this.PhaseManager = new PhaseManager();
			this.TeamManager = teamManager;

			this.PhaseManager.OnTurnEnd += () =>
			{
				foreach (BattleTeam team in teamManager.GetAllTeams())
				{
					foreach (IPlayer player in team.GetAllPlayers())
						for (int x = 0; x < player.Width; x++)
						{
							for (int y = 0; y < player.Height; y++)
								player.GetUnitSlot(new CardPosition(x, y))?.Card.Reset();
						}
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