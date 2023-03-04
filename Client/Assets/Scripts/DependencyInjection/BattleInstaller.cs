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
        [SerializeField] private BattleSystemManager battleSystemManager;

        public override void InstallBindings()
        {
            Container.BindInstance(cardDisplayFactory).AsSingle();
            Container.BindInstance(battleSystemManager).AsSingle();
            // CSVCardDatabase database = new CSVCardDatabase(textAsset.text.Trim(), true);
            // Container.Bind<ICardDatabase>().FromInstance(database).AsSingle();
        }
    }
}