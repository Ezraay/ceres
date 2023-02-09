using System;
using System.Collections;
using System.Collections.Generic;
using Ceres.Client;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay
{
    public class BattleDisplayManager : MonoBehaviour
    {
        public PlayerDisplay player;
        public PlayerDisplay opponentPlayer;

        private readonly Queue<IServerAction> actions = new();
        private IActionAnimation currentAnimation;

        private void Awake()
        {
            BattleSystemManager.OnAction += action =>
            {
                QueueAction(action);
            };
        }

        private void Update()
        {
            if (actions.Count > 0 && currentAnimation == null)
            {
                IServerAction action = actions.Dequeue();
                Logger.Log("Performing animation: " + action);
                StartCoroutine(ShowAction(action));
                return;
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                QueueAction(new DrawCardAction(new Card(CardFactory.CreateCardData("archer"))));
                QueueAction(new OpponentDrawCardAction());
            }
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
                case OpponentDrawCardAction:
                    currentAnimation = new OpponentDrawCardAnimation();
                    break;
                default:
                    Logger.LogError("No action animation found for ServerAction: " + action);
                    break;
            }

            yield return currentAnimation.GetEnumerator(action, this);

            currentAnimation = null;
        }
    }
}