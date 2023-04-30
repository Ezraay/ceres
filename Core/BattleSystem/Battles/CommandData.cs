namespace Ceres.Core.BattleSystem
{
	public struct CommandData
	{
		public ClientCommand Command;
		public IPlayer Author;
		public bool CheckCommand;

		public CommandData(ClientCommand command, IPlayer author, bool checkCommands = true)
		{
			this.Command = command;
			this.Author = author;
			this.CheckCommand = checkCommands;
		}
	}
}