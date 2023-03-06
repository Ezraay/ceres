using Ceres.Client.BattleSystem;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        // [SerializeField] private TextAsset textAsset;
        [SerializeField] private CardDisplayFactory cardDisplayFactory;
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private BattleDisplayManager battleDisplayManager;
        [SerializeField] private ActionAnimator actionAnimator;

        public override void InstallBindings()
        {
            Container.BindInstance(cardDisplayFactory);
            Container.BindInstance(battleManager);
            Container.BindInstance(battleDisplayManager);
            Container.BindInstance(actionAnimator);
        }
    }
}