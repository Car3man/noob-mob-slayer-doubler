using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Game.Configs;
using Game.Currency;
using Game.Enchantments;
using Game.Saves;

namespace Game.Upgrades
{
    public class UpgradeInventory
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameSaves _gameSaves;
        private readonly CurrencyController _currencyController;
        private readonly EnchantmentInventory _enchantmentInventory;
        private Dictionary<int, Upgrade> _upgrades;

        public delegate void UpgradeLevelChangeDelegate(int upgradeId);
        public event UpgradeLevelChangeDelegate OnUpgradeLevelChange;

        public UpgradeInventory(
            IConfigProvider configProvider,
            GameSaves gameSaves,
            CurrencyController currencyController,
            EnchantmentInventory enchantmentInventory
            )
        {
            _configProvider = configProvider;
            _gameSaves = gameSaves;
            _currencyController = currencyController;
            _enchantmentInventory = enchantmentInventory;
        }

        public void Initialize()
        {
            _upgrades = _configProvider.GetUpgrades()
                .OrderBy(upgradePrototype => upgradePrototype.Id)
                .Select(upgradePrototype => new Upgrade(upgradePrototype))
                .ToDictionary(upgrade => upgrade.Prototype.Id, upgrade => upgrade);
        }

        public void RestoreFromSave()
        {
            foreach (var upgradeId in _upgrades.Keys)
            {
                var upgrade = _upgrades[upgradeId];
                var upgradeLevel = _gameSaves.GetUpgradeLevel(upgradeId, upgrade.Prototype.StartLevel);
                upgrade.SetLevel(upgradeLevel);
            }
        }

        public IEnumerable<Upgrade> GetUpgrades() =>
            _upgrades.Select(x => x.Value).AsEnumerable();

        public Upgrade GetUpgradeById(int upgradeId) =>
            _upgrades[upgradeId];

        public BigInteger GetDamageByUpgradeId(int upgradeId)
        {
            var outDamage = GetUpgradeById(upgradeId)
                .GetDamage();
            var activeEnchantments = _enchantmentInventory.GetActiveEnchantmentsByUpgradeId(upgradeId);
            foreach (var activeEnchantment in activeEnchantments)
            {
                if (activeEnchantment.Prototype.DamageMultiplier != null) 
                {
                    outDamage *= (BigInteger)(activeEnchantment.Prototype.DamageMultiplier * 100f) / 100;
                }
            }
            return outDamage;
        }
        
        public BigInteger GetDamageByUpgradeType(UpgradeType upgradeType)
        {
            var upgradeIds = GetUpgrades()
                .Where(upgrade => upgrade.Prototype.Type == upgradeType)
                .Select(upgrade => upgrade.Prototype.Id);
            var outDamage = new BigInteger(0);
            foreach (var upgradeId in upgradeIds)
            {
                outDamage += GetDamageByUpgradeId(upgradeId);
            }
            return outDamage;
        }

        public bool IsUpgradeUnlocked(int upgradeId) =>
            upgradeId == 0 ||
            GetUpgrades().Any(upgrade => upgrade.Prototype.Id == upgradeId - 1 && upgrade.Level > upgrade.Prototype.StartLevel);

        public bool IsUpgradeMaxLevel(int upgradeId)
        {
            return GetUpgradeById(upgradeId).Level >= GetUpgradeById(upgradeId).Prototype.MaxLevel;
        }

        public bool IsEnoughCoinsToBuyUpgrade(int upgradeId)
        {
            Upgrade upgrade = GetUpgradeById(upgradeId);
            BigInteger upgradePrice = upgrade.GetUpgradePrice();
            return _currencyController.HasCoins(upgradePrice);
        }

        public void BuyUpgrade(int upgradeId)
        {
            if (!IsUpgradeUnlocked(upgradeId))
            {
                return;
            }

            if (IsUpgradeMaxLevel(upgradeId))
            {
                return;
            }

            if (!IsEnoughCoinsToBuyUpgrade(upgradeId))
            {
                return;
            }
            
            Upgrade upgrade = GetUpgradeById(upgradeId);
            BigInteger upgradePrice = upgrade.GetUpgradePrice();
            upgrade.SetLevel(upgrade.Level + 1);
            _gameSaves.SaveUpgradeLevel(upgradeId, upgrade.Level);
            _currencyController.SubtractCoins(upgradePrice);

            OnUpgradeLevelChange?.Invoke(upgradeId);
        }
        
        public void SetUpgrade(int upgradeId, int upgradeLevel) 
        {
            if (IsUpgradeMaxLevel(upgradeId))
            {
                return;
            }
            
            Upgrade upgrade = GetUpgradeById(upgradeId);
            upgrade.SetLevel(upgradeLevel);
            _gameSaves.SaveUpgradeLevel(upgradeId, upgrade.Level);

            OnUpgradeLevelChange?.Invoke(upgradeId);
        }
    }
}