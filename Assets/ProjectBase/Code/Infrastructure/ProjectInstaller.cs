using Game.Configs;
using Game.Configs.JsonImpl;
using Game.Saves;
using Services;
using ServicesImpls.PrefsSaving;
using Zenject;

namespace Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindConfigProvider();

            BindSavingManager();
            BindGameSaves();

            BindEntryPoint();
        }

        private void BindConfigProvider()
        {
            Container
                .Bind<IConfigProvider>()
                .To<JsonConfigProvider>()
                .AsSingle();
        }

        private void BindSavingManager()
        {
            Container
                .Bind<ISavingManager>()
                .FromSubContainerResolve()
                .ByInstaller<PrefsSavingInstaller>()
                .AsSingle();
        }

        private void BindGameSaves()
        {
            Container
                .Bind<GameSaves>()
                .FromNew()
                .AsSingle();
        }

        private void BindEntryPoint()
        {
            Container
                .Bind(typeof(ProjectEntryPoint), typeof(IInitializable))
                .To<ProjectEntryPoint>()
                .AsSingle()
                .NonLazy();
        }
    }
}