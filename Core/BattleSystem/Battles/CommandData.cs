namespace Ceres.Core.BattleSystem
{
	public struct CommandData
	{
		public IClientCommand Command;
		public IPlayer Author;
		public bool CheckCommand;

		public CommandData(IClientCommand command, IPlayer author, bool checkCommands = true)
		{
			this.Command = command;
			this.Author = author;
			this.CheckCommand = checkCommands;
		}
	}
}