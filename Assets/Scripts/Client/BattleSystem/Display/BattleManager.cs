using System.Collections.Generic;
using Ceres.Core.BattleSystem.Actions.PlayerActions;
using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Cards;
using Ceres.Core.BattleSystem.Players;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ceres.Client.BattleSystem.Display
{
    public class BattleManager : MonoBehaviour
    {
        private ICardData testCard;
        private ICardData testCard2;
        [SerializeField] private DisplayManager displayManager;
        private ICardDatabase cardDatabase;
        private IPlayer player1;
        private IPlayer player2;
        public Battle Battle;
        
        private void Awake()
        {
            TextAsset cardDataCSV = (TextAsset)Resources.Load("Data/Cards");
            cardDatabase = new CSVCardDatabase(cardDataCSV.text.Trim(), true);

            testCard = cardDatabase.GetCardData("archer");
            testCard2 = cardDatabase.GetCardData("spearman");
            
            player1 = new Player(CreateTestPile(), new Card(testCard));
            player2 = new Player(CreateTestPile(), new Card(testCard));
            Battle = new Battle(player1, player2);
            
            displayManager.Setup(Battle);
            
            Battle.StartBattle();
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