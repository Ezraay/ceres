using Ceres.Core.BattleSystem.Actions.PlayerActions;
using Ceres.Core.BattleSystem.Battles;
using UnityEngine;
using UnityEngine.UI;

namespace Ceres.Client.BattleSystem.Display
{
    public class BattleOverlayManager : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private Text phaseText;
        [SerializeField] private Color player1Colour = Color.red;
        [SerializeField] private Color player2Colour = Color.blue;

        private void Start()
        {
            battleManager.Battle.BattlePhaseManager.OnPhaseEnter +=
                phase => UpdatePhaseText(phase, battleManager.Battle.Player1Priority);
            UpdatePhaseText(battleManager.Battle.BattlePhaseManager.Phase, battleManager.Battle.Player1Priority);
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