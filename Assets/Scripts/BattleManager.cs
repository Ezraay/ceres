using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private CardData testCard;
        [SerializeField] private CardData testCard2;
        [SerializeField] private DisplayManager displayManager;
        private IPlayer player1;
        private IPlayer player2;
        public Battle Battle;
        
        private void Awake()
        {
            player1 = new Player(CreateTestPile(), new Card(testCard));
            player2 = new Player(CreateTestPile(), new Card(testCard));
            Battle = new Battle(player1, player2);

            //displayManager.ShowCard(new Card(testCard));
            displayManager.Setup(Battle);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                Battle.Execute(new AdvancePhase());

            Battle.Tick();
        }

        private List<ICard> CreateTestPile()
        {
            List<ICard> pile = new List<ICard>();

            for (int i = 0; i < 50; i++)
            {
                pile.Add(new Card(Random.Range(0f, 1f) < 0.5f ? testCard : testCard2));
            }

            return pile;
        }
    }
}