using System.Numerics;
using Game.Upgrades;

namespace Game.Damage
{
    public class DamageStatistics
    {
        private readonly UpgradeInventory _upgradeInventory;

        public DamageStatistics(
            UpgradeInventory upgradeInventory
            )
        {
            _upgradeInventory = upgradeInventory;
        }
        
        public BigInteger GetClickDamage()
        {
            return _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Click);
        }

        public BigInteger GetIdleDamagePerSecond()
        {
            return _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Idle);
        }
    }
}