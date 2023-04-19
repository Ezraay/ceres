#region

using System;
using System.Collections.Generic;

#endregion

namespace Ceres.Core.BattleSystem.Battles
{
	public class ServerBattle : Battle
	{
		private const int MaxDamage = 6;
		private readonly bool checkCommands;
		public readonly Guid Id;
		private readonly Queue<CommandData> stack = new Queue<CommandData>();
		private bool isExecutingStack = false;

		public ServerBattle(TeamManager teamManager, bool checkCommands = true) : base(teamManager, new PhaseManager())
		{
			this.checkCommands = checkCommands;
			this.Id = Guid.NewGuid();
		}

		public event Action<IPlayer, IServerAction> OnPlayerAction;

		private void OnPhaseEnter(BattlePhase phase)
		{
			IPlayer player = this.PhaseManager.CurrentTurnPlayer;
			switch (phase)
			{
				case BattlePhase.Stand:
					AddToStack(new AlertAllCommand(), player, false);
					AddToStack(new AdvancePhaseCommand(), player, false);
					break;
				case BattlePhase.Draw:
					AddToStack(new DrawCommand(), player, false);
					AddToStack(new AdvancePhaseCommand(), player, false);
					break;
				case BattlePhase.Defend:
					if (!this.CombatManager.ValidAttack)
						AddToStack(new AdvancePhaseCommand(), player, false);
					break;
				case BattlePhase.Damage:
					if (!this.CombatManager.ValidAttack)
						AddToStack(new AdvancePhaseCommand(), player, false);
					break;
				case BattlePhase.End:
					foreach (BattleTeam team in this.TeamManager.GetAllTeams())
					{
						foreach (IPlayer allPlayer in team.GetAllPlayers())
							AddToStack(new ResetAllUnitsCommand(), allPlayer, false);
					}

					AddToStack(new AdvancePhaseCommand(), player, false);
					break;
			}
		}

		private void ExecuteStack()
		{
			this.isExecutingStack = true;
			
			while (this.stack.Count > 0)
			{
				CommandData data = this.stack.Dequeue();
				IClientCommand command = data.Command;
				IPlayer author = data.Author;
				bool checkCommand = data.CheckCommand;
				
				if (command.CanExecute(this, author) || !this.checkCommands || !checkCommand)
				{
					command.Apply(this, author);

					BattleTeam? myTeam = this.TeamManager.GetPlayerTeam(author.Id);

					if (myTeam == null)
						return;

					foreach (IPlayer player in myTeam.GetAllPlayers())
						foreach (IServerAction action in command.GetActionsForAlly(author))
							OnPlayerAction?.Invoke(player, action);

					foreach (BattleTeam team in this.TeamManager.GetAllTeams())
						if (team != myTeam)
							foreach (IPlayer player in team.GetAllPlayers())
								foreach (IServerAction action in command.GetActionsForOpponent(author))
									OnPlayerAction?.Invoke(player, action);
				}
			}

			this.isExecutingStack = false;
		}

		public void AddToStack(IClientCommand command, IPlayer author, bool checkCommand = true)
		{
			CommandData data = new CommandData { Command = command, Author = author, CheckCommand = checkCommand };
			this.stack.Enqueue(data);
			
			if (!isExecutingStack)
				ExecuteStack();
		}
		

		public override void StartGame(List<IPlayer> playerOrder)
		{
			base.StartGame(playerOrder);

			this.TeamManager.OnCallAction += CallBattleAction;
			this.PhaseManager.OnPhaseEnter += OnPhaseEnter;

			foreach (BattleTeam team in this.TeamManager.GetAllTeams())
			{
				foreach (IPlayer player in team.GetAllPlayers())
				{
					MultiCardSlot? pile = player.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;
					if (pile == null)
						throw new Exception("Pile is hidden");
					pile.Shuffle();

					for (int i = 0; i < 5; i++)
						// player.GetMultiCardSlot(MultiCardSlotType.Hand).AddCard(pile.PopCard());
						AddToStack(new DrawCommand(), player, false);
				}
			}

			OnPhaseEnter(this.PhaseManager.Phase);
		}

		public event Action<IBattleAction> OnBattleAction;

		public void CallBattleAction(IBattleAction action)
		{
			OnBattleAction?.Invoke(action);
		}
	}
}