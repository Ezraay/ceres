﻿using System;

namespace Ceres.Core.BattleSystem
{
    public class ServerBattle : Battle
    {
        public event Action<IPlayer, IServerAction> OnPlayerAction;

        public ServerBattle(TeamManager teamManager) : base(teamManager) { }

        public void Execute(IClientCommand command, IPlayer author)
        {
            if (command.CanExecute(this, author))
            {
                command.Apply(this, author);

                BattleTeam myTeam = TeamManager.GetPlayerTeam(author.Id);

                foreach (IPlayer player in myTeam.Players)
                foreach (IServerAction action in command.GetActionsForAlly(author))
                    OnPlayerAction?.Invoke(player, action);

                foreach (var team in TeamManager.AllTeams)
                    if (team != myTeam)
                        foreach (IPlayer player in team.Players)
                        foreach (var action in command.GetActionsForOpponent(author))
                            OnPlayerAction?.Invoke(player, action);
            }
        }

        public void EndGame(string reason)
        {
            
        }
    }
}