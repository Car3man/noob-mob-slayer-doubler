using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Game.Configs;
using Game.Currency;
using Game.Saves;

namespace Game.Upgrades
{
    public class UpgradeController
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameSaves _gameSaves;
        private readonly CurrencyController _currencyController;
        private Dictionary<int, Upgrade> _upgrades;

        public delegate void BuyUpgradeDelegate(int upgradeId);
        public event BuyUpgradeDelegate OnBuyUpgrade;

        public UpgradeController(
            IConfigProvider configProvider,
            GameSaves gameSaves,
            CurrencyController currencyController
            )
        {
            _configProvider = configProvider;
            _gameSaves = gameSaves;
            _currencyController = currencyController;
        }

        public void PrepareUpgrades()
        {
            _upgrades = _configProvider.GetUpgrades()
                .OrderBy(upgradePrototype => upgradePrototype.Id)
                .Select(upgradePrototype => new Upgrade(upgradePrototype))
                .ToDictionary(upgrade => upgrade.Id, upgrade => upgrade);
        }

        public void RestoreFromSave()
        {
            foreach (var upgradeId in _upgrades.Keys)
            {
                var upgrade = _upgrades[upgradeId];
                var upgradeLevel = _gameSaves.GetUpgradeLevel(upgradeId, upgrade.StartLevel);
                upgrade.SetLevel(upgradeLevel);
            }
        }

        public IEnumerable<Upgrade> GetUpgrades() =>
            _upgrades.Select(x => x.Value).AsEnumerable();

        public Upgrade GetUpgradeById(int upgradeId) =>
            _upgrades[upgradeId];

        public BigInteger GetDamageByUpgradeId(int upgradeId)
            => GetUpgradeById(upgradeId).GetDamage();

        public BigInteger GetDamageByUpgradeType(UpgradeType upgradeType)
            => GetUpgrades()
                .Where(upgrade => upgrade.Type == upgradeType)
                .Select(upgrade => upgrade.GetDamage())
                .Aggregate(BigInteger.Add);

        public void BuyUpgrade(int upgradeId)
        {
            Upgrade upgrade = GetUpgradeById(upgradeId);
            BigInteger upgradePrice = upgrade.GetUpgradePrice();
            bool hasCoins = _currencyController.HasCoins(upgradePrice);

            if (!hasCoins)
            {
                throw new System.Exception(
                    $"Not enough coins to buy upgrade, upgradeId: {upgradeId}, upgradePrice: {upgradePrice}");
            }
            
            upgrade.SetLevel(upgrade.Level + 1);
            _gameSaves.SaveUpgradeLevel(upgradeId, upgrade.Level);
            _currencyController.SubtractCoins(upgradePrice);

            OnBuyUpgrade?.Invoke(upgradeId);
        }
    }
}