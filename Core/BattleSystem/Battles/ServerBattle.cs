using System;
using System.Collections.Generic;

namespace Ceres.Core.BattleSystem
{
	public class ServerBattle : Battle
	{
		private readonly bool checkCommands;
		public readonly Guid Id;
		public event Action<IPlayer, IServerAction> OnPlayerAction;

		public ServerBattle(TeamManager teamManager, Guid id,
			bool checkCommands = true) : base(teamManager)
		{
			this.checkCommands = checkCommands;
			this.Id = id;
		}

		public void Execute(IClientCommand command, IPlayer author)
		{
			if (command.CanExecute(this, author) || !this.checkCommands)
			{
				command.Apply(this, author);

				BattleTeam myTeam = this.TeamManager.GetPlayerTeam(author.Id);

				foreach (IPlayer player in myTeam.Players)
					foreach (IServerAction action in command.GetActionsForAlly(author))
						this.OnPlayerAction?.Invoke(player, action);

				foreach (var team in this.TeamManager.AllTeams)
					if (team != myTeam)
						foreach (IPlayer player in team.Players)
							foreach (var action in command.GetActionsForOpponent(author))
								this.OnPlayerAction?.Invoke(player, action);
			}
		}

		public void EndGame(string reason) { }

		public override void StartGame(List<IPlayer> playerOrder)
		{
			base.StartGame(playerOrder);
			
			foreach (IPlayer player in this.TeamManager.AllPlayers)
			{
				MultiCardSlot pile = player.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;
				pile.Shuffle();

				for (int i = 0; i < 5; i++)
					player.GetMultiCardSlot(MultiCardSlotType.Hand).AddCard(pile.PopCard());
			}
		}
	}
}