namespace CardGame.BattleDisplay
{
    public class AnimationData
    {
        public readonly PlayerDisplay PlayerDisplay;
        public readonly PlayerDisplay OpponentDisplay;
        public readonly CardDisplayFactory CardDisplayFactory;

        public AnimationData(PlayerDisplay playerDisplay, PlayerDisplay opponentDisplay, CardDisplayFactory cardDisplayFactory)
        {
            this.PlayerDisplay = playerDisplay;
            this.OpponentDisplay = opponentDisplay;
            CardDisplayFactory = cardDisplayFactory;
        }
    }
}