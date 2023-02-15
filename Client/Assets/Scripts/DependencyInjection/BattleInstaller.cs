using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace CardGame.BattleDisplay.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        [SerializeField] private TextAsset textAsset;

        public override void InstallBindings()
        {
            CSVCardDatabase database = new CSVCardDatabase(textAsset.text.Trim(), true);
            Container.Bind<ICardDatabase>().FromInstance(database).AsSingle();
        }
    }
}