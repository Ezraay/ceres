using System;
using System.Collections.Generic;
using CardGame.Networking;
using Ceres.Core.BattleSystem;
using Ceres.Core.BattleSystem.Battles;
using UnityEngine;

namespace CardGame.BattleDisplay.Networking
{
	public class SinglePlayerProcessor : ICommandProcessor
	{
		public ServerBattle ServerBattle { get; private set; }

		public IPlayer MyPlayer { get; private set; }
		private IPlayer myServerPlayer;

		private ClientBattle ClientBattle { get; set; }
		private readonly ServerBattleStartConfig conditions;
		public event Action<ServerAction> OnServerAction;
		public event Action<BattleStartConditions> OnStartBattle;
		public event Action<EndBattleReason> OnEndBattle;

		public SinglePlayerProcessor(ServerBattleStartConfig conditions)
		{
			this.conditions = conditions;
		}
		// private BattleTeam myTeam;

		public void Start()
		{
			IPlayer player1 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
			IPlayer player2 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
			player1.LoadDeck(this.conditions.Player1Deck);
			player2.LoadDeck(this.conditions.Player2Deck);
			this.myServerPlayer = player1;

			this.ServerBattle = new ServerBattle(player1, player2, false);
			List<IPlayer> playerOrder = new List<IPlayer> {player1, player2,};

			this.ServerBattle.OnBattleAction += OnServerBattleAction;
			this.ServerBattle.OnPlayerAction += (player, action) =>
			{
				if (player == this.myServerPlayer) // Accept actions sent to us 
				{
					this.ClientBattle.Execute(action);
					this.OnServerAction?.Invoke(action);
				}
			};

			this.ServerBattle.StartGame(playerOrder);

			this.MyPlayer = player1.GetAllyCopy();
			IPlayer enemy = player2.GetEnemyCopy();
			PhaseManager phaseManagerCopy = this.ServerBattle.PhaseManager.Copy();
			phaseManagerCopy.SetPlayers(new List<IPlayer> {this.MyPlayer, enemy,});
			this.ClientBattle = new ClientBattle(phaseManagerCopy, this.MyPlayer, enemy);

			this.OnStartBattle?.Invoke(new BattleStartConditions
			{
				ClientBattle = this.ClientBattle,
				MyPlayerId = player1.Id,
			});
		}

		private void OnServerBattleAction(IBattleAction action)
		{
			switch (action)
			{
				case EndBattleAction endGame:
					this.OnEndBattle?.Invoke(endGame.Winner == this.MyPlayer
						? EndBattleReason.YouWon
						: EndBattleReason.YouLost);
					break;
			}
		}

		public void ProcessCommand(ClientCommand command)
		{
			this.ServerBattle.AddToStack(command, this.myServerPlayer);
			Debug.Log(this.ServerBattle.PhaseManager.Phase);
		}
	}
}