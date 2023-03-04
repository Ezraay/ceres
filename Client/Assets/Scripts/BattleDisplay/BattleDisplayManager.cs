using System;
using System.Collections;
using System.Collections.Generic;
using Ceres.Client;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay
{
    public class BattleDisplayManager : MonoBehaviour
    {
        public PlayerDisplay player;
        public PlayerDisplay opponentPlayer;

        private readonly Queue<IServerAction> actions = new();
        private IActionAnimation currentAnimation;
        private CardDisplayFactory cardDisplayFactory;

        private void Awake()
        {
            BattleSystemManager.OnAction += action =>
            {
                QueueAction(action);
            };
        }

        
        [Inject]
        public void Construct(CardDisplayFactory cardDisplayFactory)
        {
            this.cardDisplayFactory = cardDisplayFactory;
        }

        private void Update()
        {
            if (actions.Count > 0 && currentAnimation == null)
            {
                IServerAction action = actions.Dequeue();
                StartCoroutine(ShowAction(action));
                return;
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

            AnimationData data = new AnimationData(player, opponentPlayer, cardDisplayFactory);
            yield return currentAnimation.GetEnumerator(action, data);

            currentAnimation = null;
        }
    }
}