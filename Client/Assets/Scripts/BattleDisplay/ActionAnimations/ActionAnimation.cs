using System.Collections;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public abstract class ActionAnimation
    {
        public abstract IEnumerator GetEnumerator(IServerAction baseAction, AnimationData data);

        protected IEnumerator StartCoroutine(AnimationData data, IEnumerator enumerator)
        {
            yield return data.ActionAnimator.StartCoroutine(enumerator);
        }
    }
}