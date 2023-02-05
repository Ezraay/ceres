using System.Collections;
using System.Collections.Generic;
using Ceres.Core.BattleSystem;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class BattleDisplayManager : MonoBehaviour
    {
        public AllyPlayerDisplay AllyPlayer;
        public OpponentPlayerDisplay OpponentPlayer;

        private readonly Queue<IServerAction> actions = new();
        private IActionAnimation currentAnimation;

        private void Update()
        {
            if (actions.Count > 0 && currentAnimation == null)
            {
                IServerAction action = actions.Dequeue();
                StartCoroutine(ShowAction(action));
                return;
            }

            if (Input.GetKeyDown(KeyCode.F1))
                QueueAction(new DrawCardAction(new Card(CardFactory.CreateCardData("archer"))));
        }

        public void QueueAction(IServerAction action)
        {
            actions.Enqueue(action);
        }

        private IEnumerator ShowAction(IServerAction action)
        {
            switch (action)
            {
                case DrawCardAction:
                    currentAnimation = new DrawCardAnimation();
                    break;
            }

            yield return currentAnimation.GetEnumerator(action, this);

            currentAnimation = null;
        }
    }
}