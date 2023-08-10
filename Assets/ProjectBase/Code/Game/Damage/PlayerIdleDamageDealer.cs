using Game.Upgrades;
using UnityEngine;
using Zenject;

namespace Game.Damage
{
    public class PlayerIdleDamageDealer : ITickable
    {
        private readonly MobDamageDealer _mobDamageDealer;
        private readonly UpgradeInventory _upgradeInventory;
        private float _lastDamageTime;

        public PlayerIdleDamageDealer(
            MobDamageDealer mobDamageDealer,
            UpgradeInventory upgradeInventory
            )
        {
            _mobDamageDealer = mobDamageDealer;
            _upgradeInventory = upgradeInventory;
        }
        
        public void Tick()
        {
            if (Time.time - _lastDamageTime >= 1f)
            {
                var idleDamage = _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Idle);
                _mobDamageDealer.DealDamage(idleDamage);

                _lastDamageTime = Time.time;
            }
        }
    }
}