using System;

namespace Ceres.Core.BattleSystem.Battles
{
    public class ServerBattle : Battle
    {
        public event Action<IPlayer, IServerAction> OnPlayerAction;
        public readonly Guid Id;
        private readonly bool checkCommands;

        public ServerBattle(TeamManager teamManager, Guid id, bool checkCommands = true) : base(teamManager)
        {
            this.checkCommands = checkCommands;
            Id = id;
        }

        public void Execute(IClientCommand command, IPlayer author)
        {
            if (command.CanExecute(this, author) || !checkCommands)
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

        public void StartGame()
        {
            foreach (BattleTeam team in TeamManager.AllTeams)
            {
                foreach (IPlayer player in team.Players)
                {
                    MultiCardSlot pile = player.GetMultiCardSlot(MultiCardSlotType.Pile) as MultiCardSlot;
                    pile.Shuffle();

                    for (int i = 0; i < 5; i++)
                        player.GetMultiCardSlot(MultiCardSlotType.Hand).AddCard(pile.PopCard());
                    
                }
            }
        }
    }
}