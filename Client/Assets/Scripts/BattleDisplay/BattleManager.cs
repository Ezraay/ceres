using System;
using CardGame.BattleDisplay.Networking;
using CardGame.Networking;
using Ceres.Core.BattleSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;
using Random = UnityEngine.Random;

namespace Ceres.Client.BattleSystem
{
    // [CreateAssetMenu(menuName = "Create BattleManager", fileName = "BattleManager", order = 0)]
    public class BattleManager : MonoBehaviour
    {
        public ClientBattle Battle { get; private set; }
        private ICommandProcessor commandProcessor;
        public IPlayer MyPlayer => commandProcessor.MyPlayer;
        public event Action<ServerAction> OnAction;
        public event Action<BattleStartConditions> OnStart;
        public event Action<EndBattleReason> OnEnd;
        public bool IsBattleOngoing { get; private set; }
        public bool IsStarted { get; private set; }

        [Inject]
        public void Construct(SceneManager sceneManager)
        {
            if (sceneManager.CurrentScene is not BattleScene battleScene)
                throw new Exception();
            this.commandProcessor = battleScene.Processor;
        }

        private void OnEnable()
        {
            this.commandProcessor.OnStartBattle += OnStartBattle;
            this.commandProcessor.OnServerAction += OnServerAction;
            this.commandProcessor.OnEndBattle += OnEndBattle;
        }

        private void OnDisable()
        {
            this.commandProcessor.OnStartBattle -= OnStartBattle;
            this.commandProcessor.OnServerAction -= OnServerAction;
            this.commandProcessor.OnEndBattle -= OnEndBattle;
        }

        private void OnStartBattle(BattleStartConditions battleStartConditions)
        {
            this.Battle = battleStartConditions.ClientBattle;
            this.OnStart?.Invoke(battleStartConditions);
            this.IsBattleOngoing = true;
            this.IsStarted = true;
        }

        private void OnEndBattle(EndBattleReason reason)
        {
            Action<EndBattleReason> action = this.OnEnd;
            action?.Invoke(reason);
            this.IsBattleOngoing = false;
        }

        private void OnServerAction(ServerAction action)
        {
            OnAction?.Invoke(action);
        }

        private void Start()
        {
            this.commandProcessor.Start();
        }

        public void Execute(ClientCommand command)
        {
            Logger.Log("Executing command: " + command);
            commandProcessor.ProcessCommand(command);
        }
    }
}