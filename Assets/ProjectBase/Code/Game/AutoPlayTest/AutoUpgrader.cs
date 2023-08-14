using System.Linq;
using Game.Islands;
using Game.Upgrades;
using Infrastructure.SceneStarters;
using Numerics;
using Zenject;

namespace Game.AutoPlayTest
{
    public class AutoUpgrader : ITickable
    {
        private readonly GameSceneStarter _gameSceneStarter;
        private readonly UpgradeInventory _upgradeInventory;
        private readonly IslandChanger _islandChanger;

        public AutoUpgrader(
            GameSceneStarter gameSceneStarter,
            UpgradeInventory upgradeInventory,
            IslandChanger islandChanger
            )
        {
            _gameSceneStarter = gameSceneStarter;
            _upgradeInventory = upgradeInventory;
            _islandChanger = islandChanger;
        }
        
        public void Tick()
        {
            if (!_gameSceneStarter.Started)
            {
                return;
            }
            
            var currentIsland = _islandChanger.CurrentIsland;
            if (currentIsland == null)
            {
                return;
            }

            var unlockedUpgrades = _upgradeInventory
                .GetUpgrades()
                .Where(upgrade => _upgradeInventory.IsUpgradeUnlocked(upgrade.Prototype.Id))
                .Where(upgrade => !_upgradeInventory.IsUpgradeMaxLevel(upgrade.Prototype.Id))
                .ToList();
                
            if (unlockedUpgrades.Count == 0)
            {
                return;
            }
            
            var limitedUpgrades = unlockedUpgrades
                .Where(upgrade => upgrade.GetUpgradePrice() < currentIsland.Prototype.MobsCoinsReward * 10)
                .ToList();
                
            var lookupUpgrades = (limitedUpgrades.Count > 0 ? limitedUpgrades : unlockedUpgrades)
                .OrderByDescending(OrderUpgradesByPriority)
                .ToList();
            
            var upgrade = lookupUpgrades.First();
            var upgradeId = upgrade.Prototype.Id;
            if (!_upgradeInventory.IsEnoughCoinsToBuyUpgrade(upgradeId))
            {
                return;
            }
            _upgradeInventory.BuyUpgrade(upgradeId);
        }

        private BigRational OrderUpgradesByPriority(Upgrade upgrade)
        {
            var upgradePrice = upgrade.GetUpgradePrice();
            var damage = upgrade.Prototype.BaseBaseDamage;
            return new BigRational(damage, upgradePrice);
        }
    }
}