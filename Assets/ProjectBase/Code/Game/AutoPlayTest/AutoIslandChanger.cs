using System.Numerics;
using Game.Configs;
using Game.Islands;
using Game.Upgrades;
using Infrastructure.SceneStarters;
using UnityEngine;
using Zenject;

namespace Game.AutoPlayTest
{
    public class AutoIslandChanger : ITickable
    {
        private readonly GameSceneStarter _gameSceneStarter;
        private readonly IConfigProvider _configProvider;
        private readonly AutoClickDamageDealer _autoClickDamageDealer;
        private readonly UpgradeInventory _upgradeInventory;
        private readonly IslandChanger _islandChanger;
        private readonly IslandCompletionSystem _islandCompletionSystem;
        
        public AutoIslandChanger(
            GameSceneStarter gameSceneStarter,
            IConfigProvider configProvider,
            AutoClickDamageDealer autoClickDamageDealer,
            UpgradeInventory upgradeInventory,
            IslandChanger islandChanger,
            IslandCompletionSystem islandCompletionSystem
            )
        {
            _gameSceneStarter = gameSceneStarter;
            _configProvider = configProvider;
            _autoClickDamageDealer = autoClickDamageDealer;
            _upgradeInventory = upgradeInventory;
            _islandChanger = islandChanger;
            _islandCompletionSystem = islandCompletionSystem;
        }
        
        public void Tick()
        {
            if (!_gameSceneStarter.Started)
            {
                return;
            }
            
            var currIsland = _islandChanger.CurrentIsland;
            if (currIsland == null)
            {
                return;
            }

            var currIslandNumber = currIsland.Prototype.Number;

            if (_islandCompletionSystem.IsCompleted(currIslandNumber) &&
                _islandChanger.GetMaxIslandNumber() == currIslandNumber)
            {
                Application.Quit();                
                return;
            }
            
            var nextIslandNumber = currIslandNumber + 1;
            if (!_islandChanger.IsIslandNumberValid(nextIslandNumber))
            {
                return;   
            }

            if (!_islandCompletionSystem.IsUnlockedOrCompleted(nextIslandNumber))
            {
                return;
            }

            var nextIsland = _configProvider.GetIslandByNumber(nextIslandNumber);
            var isNextIslandBoss = nextIsland.IsTimeLimit;
            if (isNextIslandBoss)
            {
                var damageToReach = nextIsland.MobsHealth *
                    120 / 100;
                var clickDamageForTimeLimit = _autoClickDamageDealer.GetDamagePerSecond() *
                    (BigInteger)(nextIsland.TimeLimit * 100f) / 100;
                var idleDamageForTimeLimit = _upgradeInventory.GetDamageByUpgradeType(UpgradeType.Idle) *
                    (BigInteger)(nextIsland.TimeLimit * 100f) / 100;
                
                var sumDamageForTimeLimit = clickDamageForTimeLimit + idleDamageForTimeLimit;
                if (sumDamageForTimeLimit < damageToReach)
                {
                    return;
                }
            }
            
            _islandChanger.ChangeAndSaveIsland(nextIslandNumber);
        }
    }
}