using System;
using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay
{
    public class ActionAnimator : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        
        public ActionAnimation GetAnimation(IServerAction action)
        {
            return action switch
            {
                AdvancePhaseAction => new AdvancePhaseAnimation(),
                DrawCardAction => new DrawCardAnimation(),
                OpponentDrawCardAction => new OpponentDrawCardAnimation(),
                AllySummonAction => new AllySummonAnimation(),
                OpponentSummonAction => new OpponentSummonAnimation(),
                DeclareAttackAction => new DeclareAttackAnimation(),
                SupportUnitAction => new SupportUnitAnimation(),
                _ => null
            };
        }

        public IEnumerator ShakeCamera(float duration, float amount)
        {
            yield return cameraController.Shake(duration, amount);
        }
    }
}