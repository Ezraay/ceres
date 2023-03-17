using System;
using CardGame.Networking;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay.Networking
{
    public class SinglePlayerProcessor : ICommandProcessor
    {
        private readonly ServerBattleStartConfig conditions;
        public event Action<IServerAction> OnServerAction;
        public event Action<BattleStartConditions> OnStartBattle;
        private ServerBattle serverBattle;
        private IPlayer myPlayer;
        
        public SinglePlayerProcessor(ServerBattleStartConfig conditions)
        {
            this.conditions = conditions;
        }

        public void Start()
        {
            IPlayer player1 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
            IPlayer player2 = new StandardPlayer(Guid.NewGuid(), new MultiCardSlot(), new MultiCardSlot());
            player1.LoadDeck(conditions.Player1Deck);
            player2.LoadDeck(conditions.Player2Deck);

            myPlayer = player1;

            TeamManager manager = new TeamManager();
            BattleTeam team1 = new BattleTeam(Guid.NewGuid());
            BattleTeam team2 = new BattleTeam(Guid.NewGuid());
            team1.AddPlayer(player1);
            team2.AddPlayer(player2);
            manager.AddTeam(team1);
            manager.AddTeam(team2);
            manager.MakeEnemies(team1, team2);
            
            serverBattle = new ServerBattle(manager);
            
            ClientBattle = new ClientBattle(manager.SafeCopy(player1));

            serverBattle.OnPlayerAction += (player, action) =>
            {
                if (player == player1) // Accept actions sent to us 
                {
                    ClientBattle.Execute(action);
                    OnServerAction?.Invoke(action);
                }
            };
            
            OnStartBattle?.Invoke(new BattleStartConditions()
            {
                ClientBattle = ClientBattle,
                PlayerId = player1.Id
            });
        }

        private ClientBattle ClientBattle { get; set; }

        public void ProcessCommand(IClientCommand command)
        {
            serverBattle.Execute(command, myPlayer);
        }
    }
}