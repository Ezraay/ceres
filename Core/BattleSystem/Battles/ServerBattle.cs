#region

using System;
using System.Collections.Generic;

#endregion

namespace Ceres.Core.BattleSystem.Battles
{
	public class ServerBattle : Battle
	{
		private const int MaxDamage = 6;
		private readonly IPlayer[] allPlayers;
		private readonly bool checkCommands;
		public readonly Guid Id;
		private readonly Queue<CommandData> stack = new Queue<CommandData>();
		private bool isExecutingStack;

		public ServerBattle(IPlayer player1, IPlayer player2, bool checkCommands = true) : base(new PhaseManager(),
			player1, player2)
		{
			this.checkCommands = checkCommands;
			this.Id = Guid.NewGuid();
			this.allPlayers = new[] { player1, player2 };
		}

		public event Action<IPlayer, ServerAction>? OnPlayerAction;


		private void OnPhaseEnter(BattlePhase phase)
		{
			IPlayer currentPlayer = this.PhaseManager.CurrentTurnPlayer;
			IPlayer enemy = GetEnemy(currentPlayer);
			switch (phase)
			{
				case BattlePhase.Stand:
					AddToStack(new AlertAllCommand(), currentPlayer, false);
					AddToStack(new AdvancePhaseCommand(), currentPlayer, false);
					break;
				case BattlePhase.Draw:
					AddToStack(new DrawCommand(), currentPlayer, false);
					AddToStack(new AdvancePhaseCommand(), currentPlayer, false);
					break;
				case BattlePhase.Defend:
					if (!this.CombatManager.ValidAttack)
						AddToStack(new SetPhaseCommand(BattlePhase.End), enemy, false);
					break;
				case BattlePhase.Damage:
					if (this.CombatManager.ValidAttack)
					{
						int damageCount = this.CombatManager.DamageCount(enemy.GetMultiCardSlot(MultiCardSlotType.Defense) as MultiCardSlot);
						for (int i = 0; i < damageCount; i++)
							AddToStack(new TakeDamageCommand(), enemy, false);
						this.CombatManager.Reset();
					}
					AddToStack(new SetPhaseCommand(BattlePhase.Attack), currentPlayer, false);
					break;
				case BattlePhase.End:
					foreach (IPlayer player in this.allPlayers)
						AddToStack(new ResetAllUnitsCommand(), currentPlayer, false);

					AddToStack(new AdvancePhaseCommand(), currentPlayer, false);
					break;
			}
		}

		private void ExecuteStack()
		{
			this.isExecutingStack = true;

			while (this.stack.Count > 0)
			{
				CommandData data = this.stack.Dequeue();
				ClientCommand command = data.Command;
				IPlayer author = data.Author;
				IPlayer enemy = GetEnemy(author);
				bool checkCommand = data.CheckCommand;

				if (command.CanExecute(this, author) || !this.checkCommands || !checkCommand)
					command.Apply(this, author);

				foreach (ServerAction action in command.GetActionsForAlly(author))
				{
					action.SetAuthor(author.Id);	
					OnPlayerAction?.Invoke(author, action);
				}

				foreach (ServerAction action in command.GetActionsForOpponent(author))
				{
					action.SetAuthor(author.Id);
					OnPlayerAction?.Invoke(enemy, action);
				}
			}

			this.isExecutingStack = false;
		}

		public void AddToStack(ClientCommand command, IPlayer author, bool checkCommand = true)
		{
			CommandData data = new CommandData { Command = command, Author = author, CheckCommand = checkCommand };
			this.stack.Enqueue(data);

			if (!this.isExecutingStack)
				ExecuteStack();
		}


		public override void StartGame(List<IPlayer> playerOrder)
		{
			base.StartGame(playerOrder);

			this.PhaseManager.OnPhaseEnter += OnPhaseEnter;


			foreach (IPlayer player in this.allPlayers)
			{
				MultiCardSlot? pile = player.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;
				if (pile == null)
					throw new Exception("Pile is hidden");
				pile.Shuffle();

				for (int i = 0; i < 5; i++)
					player.GetMultiCardSlot(MultiCardSlotType.Hand).AddCard(pile.PopCard());
					// AddToStack(new DrawCommand(), player, false);
			}

			// OnPhaseEnter(this.PhaseManager.Phase);
		}
		public event Action<IBattleAction> OnBattleAction;
		public void RemovePlayer(IPlayer leavingPlayer)
		{
			if (this.Player1 == leavingPlayer)
			{
				OnBattleAction?.Invoke(new EndBattleAction(this.Player2, this.Player1));
			}
		}
	}
}