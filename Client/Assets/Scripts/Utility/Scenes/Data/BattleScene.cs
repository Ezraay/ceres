using CardGame.BattleDisplay.Networking;

namespace Ceres.Client
{
	public class BattleScene : ISceneData
	{
		public string SceneName => "Battle";
		public readonly ICommandProcessor Processor;

		public BattleScene(ICommandProcessor processor)
		{
			this.Processor = processor;
		}
	}
}