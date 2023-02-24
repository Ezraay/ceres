using Zenject;

namespace Ceres.Client
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainThreadManager>().AsSingle().NonLazy();
            Container.Bind<NetworkManager>().AsSingle().NonLazy();
        }
    }
}