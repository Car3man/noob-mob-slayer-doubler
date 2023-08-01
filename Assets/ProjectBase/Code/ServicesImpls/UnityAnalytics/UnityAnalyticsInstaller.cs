using Services;
using Zenject;

namespace ServicesImpls.UnityAnalytics
{
    public class UnityAnalyticsInstaller : Installer<UnityAnalyticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAnalytics>().To<UnityAnalytics>().AsSingle().NonLazy();
        }
    }
}