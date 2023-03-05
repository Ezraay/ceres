namespace CardGame.BattleDisplay
{
    public class AnimationData
    {
        public readonly ActionAnimator ActionAnimator;
        public readonly CardDisplayFactory CardDisplayFactory;
        public readonly PlayerDisplay OpponentDisplay;
        public readonly PlayerDisplay PlayerDisplay;

        public AnimationData(PlayerDisplay playerDisplay, PlayerDisplay opponentDisplay,
            CardDisplayFactory cardDisplayFactory, ActionAnimator actionAnimator)
        {
            PlayerDisplay = playerDisplay;
            OpponentDisplay = opponentDisplay;
            CardDisplayFactory = cardDisplayFactory;
            ActionAnimator = actionAnimator;
        }
    }
}