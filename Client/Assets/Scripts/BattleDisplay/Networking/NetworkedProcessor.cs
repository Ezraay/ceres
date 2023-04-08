﻿using System;
using CardGame.Networking;
using Ceres.Client;
using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay.Networking
{
	public class NetworkedProcessor : ICommandProcessor
	{
		private ClientBattle ClientBattle { get; set; }

		public IPlayer MyPlayer { get; private set; }
		private readonly NetworkManager networkManager;
		public event Action<IServerAction> OnServerAction;
		public event Action<BattleStartConditions> OnStartBattle;
		public event Action<EndBattleReason> OnEndBattle;

		public NetworkedProcessor(NetworkManager networkManager)
		{
			this.networkManager = networkManager;
		}

		public void Start()
		{
			this.networkManager.OnStartGame += OnStartGame;
			this.networkManager.OnBattleAction += OnBattleAction;
			this.networkManager.OnGameEnd += OnBattleEnd;
		}

		private void OnStartGame(BattleStartConditions battleStartConditions)
		{
			this.ClientBattle = battleStartConditions.ClientBattle;
			if (battleStartConditions.PlayerId != null)
				this.MyPlayer = this.ClientBattle.TeamManager.GetPlayer(battleStartConditions.PlayerId);
			this.OnStartBattle?.Invoke(battleStartConditions);
		}

		private void OnBattleEnd(EndBattleReason reason)
		{
			this.networkManager.OnStartGame -= OnStartGame;
			this.networkManager.OnBattleAction -= OnBattleAction;
			this.networkManager.OnGameEnd -= OnBattleEnd;

			this.OnEndBattle?.Invoke(reason);
		}

		private void OnBattleAction(IServerAction action)
		{
			this.ClientBattle.Execute(action);
			this.OnServerAction?.Invoke(action);
		}

		public void ProcessCommand(IClientCommand command)
		{
			this.networkManager.SendCommand(command);
		}
	}
}