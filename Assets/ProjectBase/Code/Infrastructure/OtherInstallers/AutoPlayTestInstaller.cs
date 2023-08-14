using Zenject;
using Game.AutoPlayTest;
using UnityEngine;

namespace Infrastructure.OtherInstallers
{
    public class AutoPlayTestInstaller : MonoInstaller
    {
        [SerializeField] private bool useAutoPlayTest;
        
        public override void InstallBindings()
        {
            if (!useAutoPlayTest)
            {
                return;
            }

            ResetPlayerPrefs();
            ConfigureTimeScale();
            
            BindAutoClickDamageDealer();
            BindAutoIslandChanger();
            BindAutoUpgrader();
            BindAutoCoinHarvester();
            BindAutoPlayTestReporter();
        }

        private void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private void ConfigureTimeScale()
        {
            Time.timeScale = 15f;
        }

        private void BindAutoClickDamageDealer()
        {
            Container
                .Bind(typeof(AutoClickDamageDealer), typeof(ITickable))
                .To<AutoClickDamageDealer>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAutoIslandChanger()
        {
            Container
                .Bind(typeof(AutoIslandChanger), typeof(ITickable))
                .To<AutoIslandChanger>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAutoUpgrader()
        {
            Container
                .Bind(typeof(AutoUpgrader), typeof(ITickable))
                .To<AutoUpgrader>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAutoCoinHarvester()
        {
            Container
                .Bind(typeof(AutoCoinHarvester), typeof(ITickable))
                .To<AutoCoinHarvester>()
                .AsSingle()
                .NonLazy();
        }

        private void BindAutoPlayTestReporter()
        {
            Container
                .Bind(typeof(AutoPlayTestReporter), typeof(ITickable))
                .To<AutoPlayTestReporter>()
                .AsSingle()
                .NonLazy();
        }
    }
}
