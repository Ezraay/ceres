using System;
using CardGame.Networking;
using Ceres.Core.BattleSystem;
using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.Entities;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay.Networking
{
    public class SinglePlayerProcessor : ICommandProcessor
    {
        private readonly ServerBattleStartConfig conditions;
        public event Action<IServerAction> OnServerAction;
        public event Action<BattleStartConditions> OnStartBattle;
        public event Action<EndBattleReason> OnEndBattle;
        public ServerBattle ServerBattle { get; private set; }
        
        public SinglePlayerProcessor(ServerBattleStartConfig conditions)
        {
            this.conditions = conditions;
        }

        public IPlayer MyPlayer { get; private set; }
        private BattleTeam myTeam;

        public void Start()
        {
            IPlayer player1 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
            IPlayer player2 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
            player1.LoadDeck(conditions.Player1Deck);
            player2.LoadDeck(conditions.Player2Deck);
            
            TeamManager manager = new TeamManager();
            BattleTeam team1 = manager.CreateTeam();
            BattleTeam team2 = manager.CreateTeam();
            manager.AddPlayer(player1, team1);
            manager.AddPlayer(player2, team2);
            
            MyPlayer = player1;
            this.myTeam = team1;
            
            this.ServerBattle = new ServerBattle(manager, false);
            this.ServerBattle.StartGame();
            
            ClientBattle = new ClientBattle(manager.SafeCopy(player1));

            this.ServerBattle.OnPlayerAction += (player, action) =>
            {
                if (player == MyPlayer) // Accept actions sent to us 
                {
                    ClientBattle.Execute(action);
                    OnServerAction?.Invoke(action);
                }
            };
            
            this.ServerBattle.OnBattleAction += OnServerBattleAction;
            
            OnStartBattle?.Invoke(new BattleStartConditions()
            {
                ClientBattle = ClientBattle,
                PlayerId = player1.Id
            });
        }

        private void OnServerBattleAction(IBattleAction action)
        {
            switch (action)
            {
                case EndBattleAction endGame:
                    this.OnEndBattle?.Invoke(endGame.WinningTeams.Contains(this.myTeam) ? EndBattleReason.YouWon : EndBattleReason.YouLost);
                    break;
            }
        }

        private ClientBattle ClientBattle { get; set; }

        public void ProcessCommand(IClientCommand command)
        {
            this.ServerBattle.Execute(command, MyPlayer);
        }
    }
}