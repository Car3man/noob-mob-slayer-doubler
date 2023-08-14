using System.Numerics;
using Game.Damage;
using Game.Upgrades;
using Infrastructure.SceneStarters;
using UnityEngine;
using Zenject;

namespace Game.AutoPlayTest
{
    public class AutoClickDamageDealer : ITickable
    {
        private readonly GameSceneStarter _gameSceneStarter;
        private readonly DamageDealer _damageDealer;
        private readonly UpgradeInventory _upgradeInventory;

        private const int ClicksPerSecond = 4;
        
        private float _lastClickTime;

        public AutoClickDamageDealer(
            GameSceneStarter gameSceneStarter,
            DamageDealer damageDealer,
            UpgradeInventory upgradeInventory
        )
        {
            _gameSceneStarter = gameSceneStarter;
            _damageDealer = damageDealer;
            _upgradeInventory = upgradeInventory;
        }

        public void Tick()
        {
            if (!_gameSceneStarter.Started)
            {
                return;
            }
            
            if (Time.time - _lastClickTime > 1f / ClicksPerSecond)
            {
                EmulateClick();
                _lastClickTime = Time.time;
            }
        }

        public BigInteger GetDamagePerSecond()
        {
            var clickDamage = _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Click);
            return clickDamage * ClicksPerSecond;
        }

        private void EmulateClick()
        {
            var clickDamage = _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Click);
            _damageDealer.DealDamage(clickDamage, true);
        }
    }
}