using Game.Upgrades;
using UnityEngine;
using Zenject;

namespace Game.Damage
{
    public class PlayerIdleDamageDealer : ITickable
    {
        private readonly DamageDealer _damageDealer;
        private readonly UpgradeInventory _upgradeInventory;
        private float _lastDamageTime;

        public PlayerIdleDamageDealer(
            DamageDealer damageDealer,
            UpgradeInventory upgradeInventory
            )
        {
            _damageDealer = damageDealer;
            _upgradeInventory = upgradeInventory;
        }
        
        public void Tick()
        {
            const float dealDamageEvery = 1f / 5f;
            
            if (Time.time - _lastDamageTime >= dealDamageEvery)
            {
                var idleDamage = _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Idle);
                _damageDealer.DealDamage(idleDamage / 5, false);

                _lastDamageTime = Time.time;
            }
        }
    }
}