using System.Collections.Generic;
using CardGame.Client;
using UnityEngine;

namespace CardGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private CardData testCard;
        [SerializeField] private DisplayManager displayManager;
        private Player player1;
        private Player player2;
        public Battle battle;
        
        private void Start()
        {
            player1 = new Player(CreateTestPile());
            player2 = new Player(CreateTestPile());
            battle = new Battle(player1, player2);

            //displayManager.ShowCard(new Card(testCard));
            displayManager.Setup(battle);
        }

        private List<Card> CreateTestPile()
        {
            List<Card> pile = new List<Card>();

            for (int i = 0; i < 4; i++)
            {
                pile.Add(new Card(testCard));
            }

            return pile;
        }
    }
}