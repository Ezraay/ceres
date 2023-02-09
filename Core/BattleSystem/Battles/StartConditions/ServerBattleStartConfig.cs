namespace Ceres.Core.BattleSystem
{
    public class ServerBattleStartConfig
    {
        public readonly IDeck Player1Deck;
        public readonly IDeck Player2Deck;
        public readonly bool Player1Turn;

        public ServerBattleStartConfig(IDeck player1Deck, IDeck player2Deck, bool player1Turn)
        {
            Player1Deck = player1Deck;
            Player2Deck = player2Deck;
            Player1Turn = player1Turn;
        }
    }
}