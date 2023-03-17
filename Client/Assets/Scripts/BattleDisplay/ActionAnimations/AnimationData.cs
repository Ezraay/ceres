namespace CardGame.BattleDisplay
{
    public class AnimationData
    {
        public readonly ActionAnimator ActionAnimator;
        public readonly BattleDisplayManager BattleDisplayManager;
        public readonly CardDisplayFactory CardDisplayFactory;

        public AnimationData(CardDisplayFactory cardDisplayFactory, ActionAnimator actionAnimator, BattleDisplayManager battleDisplayManager)
        {
            CardDisplayFactory = cardDisplayFactory;
            ActionAnimator = actionAnimator;
            BattleDisplayManager = battleDisplayManager;
        }
    }
}