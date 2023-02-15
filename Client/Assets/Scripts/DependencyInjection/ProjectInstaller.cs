using Zenject;

namespace Ceres.Client
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainThreadManager>().AsSingle();
            Container.Bind<NetworkManager>().AsSingle().NonLazy();
        }
    }
}