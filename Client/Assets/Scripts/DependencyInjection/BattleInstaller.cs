using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        // [SerializeField] private TextAsset textAsset;
        [SerializeField] private CardDisplay cardDisplay;
        [SerializeField] private PlayerDisplay playerDisplay;
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private BattleDisplayManager battleDisplayManager;
        [SerializeField] private ActionAnimator actionAnimator;

        public override void InstallBindings()
        {
            // Container.BindInstance(cardDisplayFactory).AsSingle();
            Container.BindInstance(cardDisplay).AsSingle();
            Container.BindFactory<Card, CardDisplay, CardDisplayFactory>();
            Container.BindFactory<PlayerDisplay, PlayerDisplay.PlayerDisplayFactory>().FromComponentInNewPrefab(playerDisplay);
            Container.BindInstance(battleManager).AsSingle();
            Container.BindInstance(battleDisplayManager).AsSingle();
            Container.BindInstance(actionAnimator).AsSingle();
        }
    }
}