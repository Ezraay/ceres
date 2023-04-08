using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Networking;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay
{
	public class BattleDisplayManager : MonoBehaviour
	{
		[SerializeField] private Transform[] solo1Position;
		[SerializeField] private Transform[] solo2Position;
		[SerializeField] private Transform[] duo1Position;
		[SerializeField] private Transform[] duo2Position;

		private readonly Queue<IServerAction> actions = new Queue<IServerAction>();
		private ActionAnimator actionAnimator;
		private BattleManager battleManager;
		private CardDisplayFactory cardDisplayFactory;
		private ActionAnimation currentAnimation;

		private PlayerDisplay.PlayerDisplayFactory playerDisplayFactory;
		// [SerializeField] private PlayerDisplay playerDisplayPrefab;

		private readonly Dictionary<Guid, PlayerDisplay> playerDisplays = new Dictionary<Guid, PlayerDisplay>();
		// public PlayerDisplay player;
		// public PlayerDisplay opponentPlayer;

		public bool CanInteract => this.actions.Count == 0 && this.currentAnimation == null;

		private void Update()
		{
			if (this.actions.Count > 0 && this.currentAnimation == null)
			{
				IServerAction action = this.actions.Dequeue();
				StartCoroutine(ShowAction(action));
			}
		}

		private void OnEnable()
		{
			this.battleManager.OnAction += QueueAction;
			this.battleManager.OnStart += OnStart;
		}

		private void OnDisable()
		{
			this.battleManager.OnAction -= QueueAction;
			this.battleManager.OnStart -= OnStart;
		}

		[Inject]
		public void Construct(CardDisplayFactory cardDisplay, ActionAnimator action, BattleManager battle,
			PlayerDisplay.PlayerDisplayFactory playerFactory)
		{
			this.cardDisplayFactory = cardDisplay;
			this.actionAnimator = action;
			this.playerDisplayFactory = playerFactory;
			this.battleManager = battle;
		}

		private void OnStart(BattleStartConditions conditions)
		{
			Debug.Log("Starting battle display");
			List<BattleTeam> teams = conditions.ClientBattle.TeamManager.GetAllTeams() as List<BattleTeam>;

			for (var i = 0; i < teams.Count; i++)
			{
				BattleTeam team = teams[i];
				List<IPlayer> players = team.GetAllPlayers() as List<IPlayer>;
				Transform[] positions = players.Any(x => x.Id == conditions.PlayerId) ? players.Count == 1
						?
						this.solo1Position
						: this.duo1Position :
					players.Count == 1 ? this.solo2Position : this.duo2Position;
				for (var j = 0; j < players.Count; j++)
				{
					IPlayer player = players[j];
					Transform playerTransform = positions[j];
					PlayerDisplay newDisplay = this.playerDisplayFactory.Create();
					newDisplay.transform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
					newDisplay.transform.SetParent(this.transform);
					newDisplay.Setup(player);
					this.playerDisplays.Add(player.Id, newDisplay);
				}
			}
		}

		public void QueueAction(IServerAction action)
		{
			this.actions.Enqueue(action);
		}

		private IEnumerator ShowAction(IServerAction action)
		{
			this.currentAnimation = this.actionAnimator.GetAnimation(action);

			if (this.currentAnimation != null)
			{
				AnimationData data = new AnimationData(this.cardDisplayFactory, this.actionAnimator, this);
				yield return this.currentAnimation.GetEnumerator(action, data);
			}

			this.currentAnimation = null;
		}

		public PlayerDisplay GetPlayerDisplay(Guid playerId)
		{
			this.playerDisplays.TryGetValue(playerId, out var result);
			return result;
		}
	}
}