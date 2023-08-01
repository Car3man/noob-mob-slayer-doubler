using Game.Currency;
using Game.Damage;
using Game.Islands;
using Game.Mobs;
using Game.Upgrades;
using Ui;
using UnityEngine.Assertions;
using Zenject;

namespace Infrastructure.SceneInstallers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMobFactory();
            BindMobSpawner();
            BindMobDeathController();
            
            BindIslandFactory();
            BindIslandSpawner();

            BindMobDamageDealer();
            BindPlayerClickDamageHandler();

            BindCurrencyController();
            BindUpgradeController();

            BindGameSceneUiStarter();
        }

        private void BindMobFactory()
        {
            Container
                .BindFactory<MobPrototype, Mob, MobFactory>()
                .FromComponentInNewPrefabResource("Prefabs/Mob");
        }

        private void BindMobSpawner()
        {
            Container
                .Bind<MobSpawner>()
                .FromNew()
                .AsSingle();
        }

        private void BindMobDeathController()
        {
            Container
                .Bind<MobDeathController>()
                .FromNew()
                .AsSingle();
        }

        private void BindIslandFactory()
        {
            Container
                .BindFactory<IslandPrototype, Island, IslandFactory>()
                .FromComponentInNewPrefabResource("Prefabs/Island");
        }

        private void BindIslandSpawner()
        {
            Container
                .Bind<IslandSpawner>()
                .FromNew()
                .AsSingle();
        }

        private void BindMobDamageDealer()
        {
            Container
                .Bind<MobDamageDealer>()
                .FromNew()
                .AsSingle();
        }

        private void BindPlayerClickDamageHandler()
        {
            var playerClickDamageHandlerInScene = FindObjectOfType<PlayerClickDamageDealer>();
            Assert.IsNotNull(playerClickDamageHandlerInScene);
            
            Container
                .Bind<PlayerClickDamageDealer>()
                .FromInstance(playerClickDamageHandlerInScene)
                .AsSingle();
        }

        private void BindCurrencyController()
        {
            Container
                .Bind<CurrencyController>()
                .FromNew()
                .AsSingle();
        }

        private void BindUpgradeController()
        {
            Container
                .Bind<UpgradeController>()
                .FromNew()
                .AsSingle();
        }

        private void BindGameSceneUiStarter()
        {
            var gameSceneUiStarter = FindObjectOfType<GameSceneUiStarter>();
            Assert.IsNotNull(gameSceneUiStarter);
            
            Container
                .Bind<GameSceneUiStarter>()
                .FromInstance(gameSceneUiStarter)
                .AsSingle();
        }
    }
}