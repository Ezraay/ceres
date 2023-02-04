using System.Collections;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame
{
    public class ActionAnimator : MonoBehaviour
    {
        private Queue<IServerAction> actions = new();
        private IEnumerator currentAnimation;

        public void Queue(IServerAction action)
        {
            actions.Enqueue(action);
        }

        private void Update()
        {
            if (actions.Count > 0)
            {
                
            }
        }
    }
}