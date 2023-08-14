using Game.Cheats;
using Game.Currency;
using Game.Damage;
using Game.Enchantments;
using Game.Islands;
using Game.Mobs;
using Game.Upgrades;
using Infrastructure.SceneStarters;
using Ui;
using Ui.Panels;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Infrastructure.SceneInstallers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameSceneCamera();
            
            BindMobFactory();
            BindMobSpawner();
            BindMobDeathSpawner();
            
            BindIslandFactory();
            BindIslandSpawner();
            BindIslandChanger();
            BindIslandCompletionSystem();
            BindIslandLimitTimer();

            BindDamageDealer();
            BindPlayerClickDamageHandler();
            BindPlayerIdleDamageDealer();

            BindCurrencyController();
            BindCoinPool();
            BindCoinSpawner();
            BindCoinHarvester();

            BindEnchantmentInventory();
            BindUpgradeInventory();
            
            BindGameSceneUiStarter();
            BindUiItemsFactories();
            
            BindGameSceneStarter();

            BindCheats();
        }

        private void BindGameSceneCamera()
        {
            var gameSceneCamera = FindObjectOfType<Camera>();
            Assert.IsNotNull(gameSceneCamera);
            
            Container
                .Bind<Camera>()
                .FromInstance(gameSceneCamera)
                .AsSingle();
        }

        private void BindMobFactory()
        {
            Container
                .BindFactory<MobPrototype, Mob, MobFactory>()
                .FromMethod(CreateMob);
            
            Mob CreateMob(DiContainer container, MobPrototype mobPrototype)
            {
                return container.InstantiatePrefabResourceForComponent<Mob>(
                    $"Prefabs/Mobs/{mobPrototype.ResId}", new object[] { mobPrototype });
            }
        }

        private void BindMobSpawner()
        {
            Container
                .Bind<MobSpawner>()
                .FromNew()
                .AsSingle();
        }

        private void BindMobDeathSpawner()
        {
            Container
                .Bind<MobDeathHandler>()
                .FromNew()
                .AsSingle();
        }

        private void BindIslandFactory()
        {
            Container
                .BindFactory<IslandPrototype, Island, IslandFactory>()
                .FromMethod(CreateIsland);

            Island CreateIsland(DiContainer container, IslandPrototype islandPrototype)
            {
                return container.InstantiatePrefabResourceForComponent<Island>(
                    $"Prefabs/Islands/{islandPrototype.ResId}", new object[] { islandPrototype });
            }
        }

        private void BindIslandSpawner()
        {
            Container
                .Bind<IslandSpawner>()
                .FromNew()
                .AsSingle();
        }

        private void BindIslandChanger()
        {
            Container
                .Bind<IslandChanger>()
                .FromNew()
                .AsSingle();
        }

        private void BindIslandCompletionSystem()
        {
            Container
                .Bind<IslandCompletionSystem>()
                .FromNew()
                .AsSingle();
        }

        private void BindIslandLimitTimer()
        {
            Container
                .Bind<IslandLimitTimer>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private void BindDamageDealer()
        {
            Container
                .Bind<DamageDealer>()
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

        private void BindPlayerIdleDamageDealer()
        {
            Container
                .Bind(typeof(PlayerIdleDamageDealer), typeof(ITickable))
                .To<PlayerIdleDamageDealer>()
                .AsSingle();
        }

        private void BindCurrencyController()
        {
            Container
                .Bind<CurrencyController>()
                .FromNew()
                .AsSingle();
        }

        private void BindCoinPool()
        {
            Container
                .BindMemoryPool<Coin, CoinPool>()
                .FromComponentInNewPrefabResource("Prefabs/Coin");
        }

        private void BindCoinSpawner()
        {
            Container
                .Bind<CoinSpawner>()
                .To<CoinSpawner>()
                .AsSingle()
                .NonLazy();
        }

        private void BindCoinHarvester()
        {
            Container
                .Bind(typeof(CoinHarvester), typeof(ITickable), typeof(IFixedTickable))
                .To<CoinHarvester>()
                .AsSingle();
        }

        private void BindEnchantmentInventory()
        {
            Container
                .Bind<EnchantmentInventory>()
                .FromNew()
                .AsSingle();
        }

        private void BindUpgradeInventory()
        {
            Container
                .Bind<UpgradeInventory>()
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

        private void BindUiItemsFactories()
        {
            Container
                .BindFactory<UpgradeItem, UpgradeItem.Factory>()
                .FromComponentInNewPrefabResource("Prefabs/Ui/Items/UpgradeItem");
        }

        private void BindGameSceneStarter()
        {
            var gameSceneStarter = FindObjectOfType<GameSceneStarter>();
            Assert.IsNotNull(gameSceneStarter);
            
            Container
                .Bind<GameSceneStarter>()
                .FromInstance(gameSceneStarter)
                .AsSingle();
        }

        private void BindCheats()
        {
            Container
                .Bind(typeof(CheatsController), typeof(ITickable))
                .To<CheatsController>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}