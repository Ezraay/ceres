using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Ceres.Client.BattleSystem
{
    public class BattlePhaseDisplay : MonoBehaviour
    {
        [SerializeField] private Text phaseText;

        private void Start()
        {
            BattleManager.Battle.PhaseManager.OnPhaseEnter += OnPhaseEnter;
        }

        private void OnPhaseEnter(BattlePhase phase)
        {
            phaseText.text = phase.ToString();
        }

        public void NextPhaseButton()
        {
            BattleManager.Execute(new TestDrawCommand());
        }
    }
}