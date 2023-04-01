using System;
using System.Collections.Generic;
using System.Linq;
using Ceres.Core.Entities;

namespace Ceres.Core.BattleSystem.Battles
{
    public class ServerBattle : Battle
    {
        public event Action<IPlayer, IServerAction> OnPlayerAction;
        public readonly Guid Id;
        private readonly bool checkCommands;
        private const int MaxDamage = 6;

        public ServerBattle(TeamManager teamManager, bool checkCommands = true) : base(teamManager)
        {
            this.checkCommands = checkCommands;
            Id = Guid.NewGuid();
            
            teamManager.OnCallAction += CallBattleAction;
        }

        public void Execute(IClientCommand command, IPlayer author)
        {
            if (command.CanExecute(this, author) || !checkCommands)
            {
                command.Apply(this, author);

                BattleTeam? myTeam = TeamManager.GetPlayerTeam(author.Id);

                if (myTeam == null) 
                    return;

                foreach (IPlayer player in myTeam.GetAllPlayers())
                foreach (IServerAction action in command.GetActionsForAlly(author))
                    OnPlayerAction?.Invoke(player, action);

                foreach (BattleTeam team in TeamManager.GetAllTeams())
                    if (team != myTeam)
                        foreach (IPlayer player in team.GetAllPlayers())
                        foreach (var action in command.GetActionsForOpponent(author))
                            OnPlayerAction?.Invoke(player, action);
            }
        }

        public void StartGame()
        {
            foreach (BattleTeam team in TeamManager.GetAllTeams())
            {
                foreach (IPlayer player in team.GetAllPlayers())
                {
                    MultiCardSlot? pile = player.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;
                    if (pile == null)
                        throw new Exception("Pile is hidden");
                    pile.Shuffle();

                    for (int i = 0; i < 5; i++)
                        player.GetMultiCardSlot(MultiCardSlotType.Hand).AddCard(pile.PopCard());
                    
                }
            }
        }

        public event Action<IBattleAction> OnBattleAction;

        public void CallBattleAction(IBattleAction action)
        {
            OnBattleAction?.Invoke(action);
        }
    }
}