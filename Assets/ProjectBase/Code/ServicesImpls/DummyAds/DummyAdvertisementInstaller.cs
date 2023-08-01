using Services;
using Zenject;

namespace ServicesImpls.DummyAds
{
    public class DummyAdvertisementInstaller : Installer<DummyAdvertisementInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAdvertisement>().To<DummyAdvertisement>().AsSingle();
        }
    }
}