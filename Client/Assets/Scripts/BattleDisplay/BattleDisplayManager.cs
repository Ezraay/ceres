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

        public bool CanInteract => actions.Count == 0 && currentAnimation == null;
        
        private readonly Queue<IServerAction> actions = new();
        private ActionAnimation currentAnimation;
        private CardDisplayFactory cardDisplayFactory;
        private ActionAnimator actionAnimator;
        
        [Inject]
        public void Construct(CardDisplayFactory cardDisplay, ActionAnimator action, BattleManager battleManager)
        {
            cardDisplayFactory = cardDisplay;
            actionAnimator = action;
            battleManager.OnAction += QueueAction;
            battleManager.OnStartBattle += StartBattle;
        }

        private void StartBattle(ClientBattle battle)
        {
            player.Champion.Setup(battle.AllyPlayer.Champion);
        }

        private void Update()
        {
            if (actions.Count > 0 && currentAnimation == null)
            {
                IServerAction action = actions.Dequeue();
                StartCoroutine(ShowAction(action));
            }
        }

        public void QueueAction(IServerAction action)
        {
            actions.Enqueue(action);
        }

        private IEnumerator ShowAction(IServerAction action)
        {
            currentAnimation = actionAnimator.GetAnimation(action);

            AnimationData data = new AnimationData(player, opponentPlayer, cardDisplayFactory, actionAnimator);
            yield return currentAnimation.GetEnumerator(action, data);

            currentAnimation = null;
        }
    }
}