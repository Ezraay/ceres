using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class BattleOverlayManager : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private Text phaseText;
        [SerializeField] private Color player1Colour = Color.red;
        [SerializeField] private Color player2Colour = Color.blue;

        private void Start()
        {
            battleManager.Battle.Phase.OnPhaseEnter +=
                phase => UpdatePhaseText(phase, battleManager.Battle.Player1Priority);
            UpdatePhaseText(battleManager.Battle.Phase.Value, battleManager.Battle.Player1Priority);
        }

        public void NextPhaseButton()
        {
            battleManager.Battle.Execute(new AdvancePhase());
        }

        private void UpdatePhaseText(BattlePhase phase, bool player1Turn)
        {
            phaseText.text = phase.ToString();
            phaseText.color = player1Turn ? player1Colour : player2Colour;
        }
    }
}