using Game.Currency;
using Game.Enchantments;
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
        private EnchantmentInventory _enchantmentInventory;
        private IslandChanger _islandChanger;
        private GameSceneUiStarter _gameSceneUiStarter;
        
        public bool Started { get; private set; }

        [Zenject.Inject]
        public void Construct(
            CurrencyController currencyController,
            UpgradeInventory upgradeInventory,
            EnchantmentInventory enchantmentInventory,
            IslandChanger islandChanger,
            GameSceneUiStarter gameSceneUiStarter
            )
        {
            _currencyManager = currencyController;
            _upgradeInventory = upgradeInventory;
            _enchantmentInventory = enchantmentInventory;
            _islandChanger = islandChanger;
            _gameSceneUiStarter = gameSceneUiStarter;
        }

        protected override void OnStart()
        {
            _upgradeInventory.Initialize();
            _enchantmentInventory.Initialize();
            
            _currencyManager.RestoreFromSave();
            _upgradeInventory.RestoreFromSave();
            _enchantmentInventory.RestoreFromSave();
            _islandChanger.RestoreFromSave();
            
            _gameSceneUiStarter.OnGameStart();

            Started = true;
        }
    }
}