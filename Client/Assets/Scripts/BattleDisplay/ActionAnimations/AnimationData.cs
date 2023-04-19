using Ceres.Core.BattleSystem;

namespace CardGame.BattleDisplay
{
    public class AnimationData
    {
        public readonly ActionAnimator ActionAnimator;
        public readonly BattleDisplayManager BattleDisplayManager;
        public readonly CardDisplayFactory CardDisplayFactory;
        public readonly BattleHUD BattleHUD;
        public readonly ClientBattle ClientBattle;

        public AnimationData(CardDisplayFactory cardDisplayFactory, ActionAnimator actionAnimator, BattleDisplayManager battleDisplayManager, BattleHUD battleHUD, ClientBattle clientBattle)
        {
            CardDisplayFactory = cardDisplayFactory;
            ActionAnimator = actionAnimator;
            BattleDisplayManager = battleDisplayManager;
            this.BattleHUD = battleHUD;
            this.ClientBattle = clientBattle;
        }
    }
}