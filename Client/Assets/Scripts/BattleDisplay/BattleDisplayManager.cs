using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Networking;
using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CardGame.BattleDisplay
{
	public class BattleDisplayManager : MonoBehaviour
	{
		[FormerlySerializedAs("solo1Position"),SerializeField] private Transform player1Position;
		[FormerlySerializedAs("solo2Position"),SerializeField] private Transform player2Position;
		[SerializeField] private BattleHUD battleHUD;

		private readonly Queue<ServerAction> actions = new Queue<ServerAction>();
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
				ServerAction action = this.actions.Dequeue();
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
			// List<BattleTeam> teams = conditions.ClientBattle.TeamManager.GetAllTeams() as List<BattleTeam>;
			SetupPlayer(conditions.ClientBattle.Player1, this.player1Position);
			SetupPlayer(conditions.ClientBattle.Player2, this.player2Position);

			// for (var i = 0; i < teams.Count; i++)
			// {
				// BattleTeam team = teams[i];
				// List<IPlayer> players = team.GetAllPlayers() as List<IPlayer>;
				// Transform[] positions = players.Any(x => x.Id == conditions.MyPlayerId) ? players.Count == 1
				// 		?
				// 		this.player1Position
				// 		: this.duo1Position :
				// 	players.Count == 1 ? this.player2Position : this.duo2Position;
				
				// for (var j = 0; j < players.Count; j++)
				// {
					// IPlayer player = players[j];
					// Transform playerTransform = positions[j];
					// PlayerDisplay newDisplay = this.playerDisplayFactory.Create();
					// newDisplay.transform.SetPositionAndRotation(playerTransform.position, playerTransform.rotation);
					// newDisplay.transform.SetParent(this.transform);
					// newDisplay.Setup(player);
					// this.playerDisplays.Add(player.Id, newDisplay);
				// }
			// }
		}

		private void SetupPlayer(IPlayer player, Transform positionParent)
		{
			PlayerDisplay newDisplay = this.playerDisplayFactory.Create();
			newDisplay.transform.SetPositionAndRotation(positionParent.position, positionParent.rotation);
			newDisplay.transform.SetParent(this.transform);
			newDisplay.Setup(player);
			this.playerDisplays.Add(player.Id, newDisplay);
		}

		public void QueueAction(ServerAction action)
		{
			this.actions.Enqueue(action);
		}

		private IEnumerator ShowAction(ServerAction action)
		{
			this.currentAnimation = this.actionAnimator.GetAnimation(action);

			if (this.currentAnimation != null)
			{
				AnimationData data = new AnimationData(this.cardDisplayFactory, this.actionAnimator, this, this.battleHUD, this.battleManager.Battle);
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