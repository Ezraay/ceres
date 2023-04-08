using CardGame.BattleDisplay.Networking;

namespace Ceres.Client
{
	public class BattleScene : SceneData
	{
		public override string SceneName => "Battle";
		public readonly ICommandProcessor Processor;

		public BattleScene(ICommandProcessor processor)
		{
			this.Processor = processor;
		}
	}
}