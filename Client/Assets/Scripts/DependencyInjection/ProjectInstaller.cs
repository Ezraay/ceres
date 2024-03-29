﻿using Ceres.Core.BattleSystem;
using UnityEngine;
using Zenject;

namespace Ceres.Client
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private TextAsset cardCsv;
        [SerializeField] private TextAsset deckCsv;
        [SerializeField] private MainThreadManager mainThreadManager;
        [SerializeField] private NetworkManager networkManager;
        
        public override void InstallBindings()
        {
            CSVCardDatabase database = new CSVCardDatabase(cardCsv.text.Trim(), true);
            CSVDeck deck = new CSVDeck(database, deckCsv.text.Trim(), true);

            Container.Bind<ICardDatabase>().FromInstance(database);
            Container.Bind<IDeck>().FromInstance(deck);
            
            Container.BindInterfacesAndSelfTo<MainThreadManager>().FromComponentInNewPrefab(mainThreadManager).AsSingle().NonLazy();
            Container.Bind<NetworkManager>().FromComponentInNewPrefab(networkManager).AsSingle().NonLazy();
        }
    }
}