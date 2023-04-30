#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Ceres.Core.BattleSystem
{
	public class PhaseManager
	{
		private readonly BattlePhase FirstPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Min();
		private readonly BattlePhase LastPhase = Enum.GetValues(typeof(BattlePhase)).Cast<BattlePhase>().Max();
		private int turn;
		private List<IPlayer> players;

		public PhaseManager(BattlePhase? phase = null)
		{
			this.Phase = phase ?? this.FirstPhase;
		}

		public BattlePhase Phase { get; private set; }
		public IPlayer CurrentTurnPlayer => this.players[this.turn % this.players.Count];

		public event Action OnTurnEnd;
		public event Action<BattlePhase> OnPhaseExit;
		public event Action<BattlePhase> OnPhaseEnter;

		public void SetPlayers(List<IPlayer> players)
		{
			this.players = players;
		}

		public void Set(BattlePhase phase)
		{
			OnPhaseExit?.Invoke(this.Phase);
			if (phase <= this.Phase) OnTurnEnd?.Invoke();
			this.Phase = phase;

			OnPhaseEnter?.Invoke(this.Phase);
		}

		public void Advance()
		{
			OnPhaseExit?.Invoke(this.Phase);
			if (this.Phase == this.LastPhase)
			{
				this.Phase = this.FirstPhase;
				this.turn++;
				OnTurnEnd?.Invoke();
			}
			else
			{
				this.Phase++;
			}

			OnPhaseEnter?.Invoke(this.Phase);
		}

		public PhaseManager Copy()
		{
			PhaseManager copy = new PhaseManager(this.Phase);
			copy.Set(this.Phase);
			return copy;
		}
	}
}