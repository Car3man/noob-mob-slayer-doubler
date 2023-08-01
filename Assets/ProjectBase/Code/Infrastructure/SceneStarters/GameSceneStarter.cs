using Game.Currency;
using Game.Islands;
using Game.Mobs;
using Game.Upgrades;
using Ui;

namespace Infrastructure.SceneStarters
{
    public class GameSceneStarter : BaseSceneStarter
    {
        private CurrencyController _currencyManager;
        private UpgradeController _upgradeController;
        private IslandSpawner _islandSpawner;
        private MobSpawner _mobSpawner;
        private GameSceneUiStarter _gameSceneUiStarter;

        [Zenject.Inject]
        public void Construct(
            CurrencyController currencyController,
            UpgradeController upgradeController,
            IslandSpawner islandSpawner,
            MobSpawner mobSpawner,
            GameSceneUiStarter gameSceneUiStarter
            )
        {
            _upgradeController = upgradeController;
            _currencyManager = currencyController;
            _islandSpawner = islandSpawner;
            _mobSpawner = mobSpawner;
            _gameSceneUiStarter = gameSceneUiStarter;
        }

        protected override void OnStart()
        {
            _currencyManager.RestoreFromSave();
            _upgradeController.PrepareUpgrades();
            _upgradeController.RestoreFromSave();
            _islandSpawner.RestoreFromSave();
            _mobSpawner.SpawnNext();
            _gameSceneUiStarter.OnGameStart();
        }
    }
}