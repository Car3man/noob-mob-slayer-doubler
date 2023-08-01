using Services;
using Zenject;

namespace ServicesImpls.PrefsSaving
{
    public class PrefsSavingInstaller : Installer<PrefsSavingInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISavingManager>().To<PrefsSavingManager>().AsSingle();
        }
    }
}
