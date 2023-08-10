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
        private UpgradeInventory _upgradeInventory;
        private IslandChanger _islandChanger;
        private MobSpawner _mobSpawner;
        private GameSceneUiStarter _gameSceneUiStarter;

        [Zenject.Inject]
        public void Construct(
            CurrencyController currencyController,
            UpgradeInventory upgradeInventory,
            IslandChanger islandChanger,
            MobSpawner mobSpawner,
            GameSceneUiStarter gameSceneUiStarter
            )
        {
            _upgradeInventory = upgradeInventory;
            _currencyManager = currencyController;
            _islandChanger = islandChanger;
            _mobSpawner = mobSpawner;
            _gameSceneUiStarter = gameSceneUiStarter;
        }

        protected override void OnStart()
        {
            _currencyManager.RestoreFromSave();
            _upgradeInventory.PrepareUpgrades();
            _upgradeInventory.RestoreFromSave();
            _islandChanger.RestoreFromSave();
            _mobSpawner.SpawnNext();
            _gameSceneUiStarter.OnGameStart();
        }
    }
}