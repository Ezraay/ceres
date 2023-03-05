using System.Collections;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public abstract class ActionAnimation
    {
        public abstract IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data);

        protected void StartCoroutine(AnimationData data, IEnumerator enumerator)
        {
            data.ActionAnimator.StartCoroutine(enumerator);
        }
    }
}