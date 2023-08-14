using Game.Currency;
using Game.Islands;
using Game.Upgrades;
using UnityEngine;
using Zenject;

namespace Game.Cheats
{
    public class CheatsController : ITickable
    {
        private readonly CurrencyController _currencyController;
        private readonly UpgradeInventory _upgradeInventory;
        private readonly IslandCompletionSystem _islandCompletionSystem;
        private readonly IslandChanger _islandChanger;

        public CheatsController(
            CurrencyController currencyController,
            UpgradeInventory upgradeInventory,
            IslandCompletionSystem islandCompletionSystem,
            IslandChanger islandChanger
            )
        {
            _currencyController = currencyController;
            _upgradeInventory = upgradeInventory;
            _islandCompletionSystem = islandCompletionSystem;
            _islandChanger = islandChanger;
        }
        
        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                StartFromIsland2();
            }
        }

        private void StartFromIsland2()
        {
            for (int i = 1; i <= 10; i++)
            {
                _islandCompletionSystem.ForceUnlock(i);
            }
            
            _currencyController.SetCoins(0);
            _upgradeInventory.SetUpgrade(0, 13);
            _upgradeInventory.SetUpgrade(1, 8);
            _upgradeInventory.SetUpgrade(2, 2);
            
            _islandChanger.ChangeAndSaveIsland(11);
        }
    }
}
