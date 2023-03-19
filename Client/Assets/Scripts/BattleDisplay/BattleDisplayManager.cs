using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Networking;
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
        // [SerializeField] private PlayerDisplay playerDisplayPrefab;

        private Dictionary<Guid, PlayerDisplay> playerDisplays = new Dictionary<Guid, PlayerDisplay>();
        // public PlayerDisplay player;
        // public PlayerDisplay opponentPlayer;

        public bool CanInteract => actions.Count == 0 && currentAnimation == null;
        
        private readonly Queue<IServerAction> actions = new();
        private PlayerDisplay.PlayerDisplayFactory playerDisplayFactory;
        private ActionAnimation currentAnimation;
        private CardDisplayFactory cardDisplayFactory;
        private ActionAnimator actionAnimator;
        [SerializeField] private Transform[] solo1Position;
        [SerializeField] private Transform[] solo2Position;
        [SerializeField] private Transform[] duo1Position;
        [SerializeField] private Transform[] duo2Position;
        
        [Inject]
        public void Construct(CardDisplayFactory cardDisplay, ActionAnimator action, BattleManager battleManager, PlayerDisplay.PlayerDisplayFactory playerFactory)
        {
            cardDisplayFactory = cardDisplay;
            actionAnimator = action;
            playerDisplayFactory = playerFactory;
            battleManager.OnAction += QueueAction;
            battleManager.OnStartBattle += StartBattle;
        }

        private void StartBattle(BattleStartConditions conditions)
        {
            for (var i = 0; i < conditions.ClientBattle.TeamManager.AllTeams.Count; i++)
            {
                BattleTeam team = conditions.ClientBattle.TeamManager.AllTeams[i];
                Transform[] positions = team.Players.Any(x => x.Id == conditions.PlayerId) ? 
                    team.Players.Count == 1 ? solo1Position : duo1Position :
                    team.Players.Count == 1 ? solo2Position : duo2Position;
                for (var j = 0; j < team.Players.Count; j++)
                {
                    IPlayer player = team.Players[j];
                    Transform playerTransform = positions[j];
                    PlayerDisplay newDisplay = playerDisplayFactory.Create();
                    newDisplay.transform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
                    newDisplay.transform.SetParent(transform);
                    newDisplay.Setup(player);
                    playerDisplays.Add(player.Id, newDisplay);
                }
            }
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

            AnimationData data = new AnimationData(cardDisplayFactory, actionAnimator, this);
            yield return currentAnimation.GetEnumerator(action, data);

            currentAnimation = null;
        }

        public PlayerDisplay GetPlayerDisplay(Guid playerId)
        {
            playerDisplays.TryGetValue(playerId, out var result);
            return result;
        }
    }
}