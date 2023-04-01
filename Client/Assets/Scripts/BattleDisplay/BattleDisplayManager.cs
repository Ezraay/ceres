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
            List<BattleTeam> teams = conditions.ClientBattle.TeamManager.GetAllTeams() as List<BattleTeam>;

            for (var i = 0; i < teams.Count; i++)
            {
                BattleTeam team = teams[i];
                List<IPlayer> players = team.GetAllPlayers() as List<IPlayer>;
                Transform[] positions = players.Any(x => x.Id == conditions.PlayerId) ? 
                    players.Count == 1 ? solo1Position : duo1Position :
                    players.Count == 1 ? solo2Position : duo2Position;
                for (var j = 0; j < players.Count; j++)
                {
                    IPlayer player = players[j];
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

            if (currentAnimation != null)
            {
                AnimationData data = new AnimationData(cardDisplayFactory, actionAnimator, this);
                yield return currentAnimation.GetEnumerator(action, data);
            }

            currentAnimation = null;
        }

        public PlayerDisplay GetPlayerDisplay(Guid playerId)
        {
            playerDisplays.TryGetValue(playerId, out var result);
            return result;
        }
    }
}