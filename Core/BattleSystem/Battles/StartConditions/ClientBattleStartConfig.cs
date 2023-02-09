namespace Ceres.Core.BattleSystem
{
    public class ClientBattleStartConfig
    {
        public readonly CardData Champion;
        public readonly bool MyTurn;
        public readonly int OpponentPileCount;
        public readonly int PileCount;

        public ClientBattleStartConfig(CardData champion, bool myTurn, int pileCount, int opponentPileCount)
        {
            Champion = champion;
            PileCount = pileCount;
            MyTurn = myTurn;
            OpponentPileCount = opponentPileCount;
        }
    }
}