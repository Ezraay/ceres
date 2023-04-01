namespace Ceres.Core.BattleSystem
{
	public class AddDefenderCommand : IClientCommand
	{
		public bool CanExecute(ClientBattle battle, IPlayer author)
		{
			throw new System.NotImplementedException();
		}

		public bool CanExecute(ServerBattle battle, IPlayer author)
		{
			throw new System.NotImplementedException();
		}

		public void Apply(ServerBattle battle, IPlayer author)
		{
			throw new System.NotImplementedException();
		}

		public IServerAction[] GetActionsForAlly(IPlayer author)
		{
			throw new System.NotImplementedException();
		}

		public IServerAction[] GetActionsForOpponent(IPlayer author)
		{
			throw new System.NotImplementedException();
		}
	}
}