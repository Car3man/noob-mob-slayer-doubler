using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Islands;
using Game.Mobs;
using UnityEngine;
using Utility;
using Vector3 = UnityEngine.Vector3;

namespace Game.Currency
{
    public class CoinSpawner
    {
        private readonly CoinPool _coinPool;
        private readonly IslandSpawner _islandSpawner;

        private readonly List<Coin> _coins = new();
        private readonly Dictionary<Coin, float> _coinSpawnTime = new();
        private readonly Dictionary<Coin, CancellationTokenSource> _coinCancellationTokens = new();
        private readonly Dictionary<Coin, bool> _coinDropAnimationFlags = new();

        public ReadOnlyCollection<Coin> Coins => _coins.AsReadOnly();

        public CoinSpawner(
            CoinPool coinPool,
            IslandSpawner islandSpawner,
            MobDeathHandler mobDeathHandler
            )
        {
            _coinPool = coinPool;
            _islandSpawner = islandSpawner;
            mobDeathHandler.OnMobDeath += OnMobDeath;
        }
        
        private void OnMobDeath(Mob mob)
        {
            SpawnFromMobDeath(mob);
        }

        private void SpawnFromMobDeath(Mob mob)
        {
            const int minCountParts = 3;
            const int maxCountParts = 9;
            var countParts = Random.Range(minCountParts, maxCountParts + 1);
            
            var sumCoinReward = GetSumOfCoinsRewardForMob(mob);
            if (sumCoinReward <= 0)
            {
                return;
            }
            
            var partCoinReward = sumCoinReward / countParts;
            for (int i = 0; i < countParts; i++)
            {
                var coinReward = partCoinReward;
                
                if (i == countParts - 1)
                {
                    coinReward += sumCoinReward - partCoinReward * countParts;
                }
                
                if (coinReward <= 0)
                {
                    continue;
                }

                var coinInstance = Spawn(coinReward);
                UniTask.Create(() => AnimateCoinDropOutFromMob(mob, coinInstance));
            }
        }

        private async UniTask AnimateCoinDropOutFromMob(Mob mob, Coin coin)
        {
            const float dropOutDuration = 0.33f;

            _coinDropAnimationFlags[coin] = true;
            
            var dropCurveStartPoint = mob.transform.position;
            var dropCurveDestinationPoint = GetNextCoinDropDestinationPoint(mob);
            var dropCurveControlPoint = dropCurveDestinationPoint + Vector3.up * 2f;

            float timeDown = dropOutDuration;
            while (timeDown > 0f)
            {
                var animationCurvePoint = BezierCurve.Point3(1f - timeDown / dropOutDuration, new List<Vector3>()
                {
                    dropCurveStartPoint, dropCurveControlPoint, dropCurveDestinationPoint
                });
                coin.transform.position = animationCurvePoint;
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, _coinCancellationTokens[coin].Token);
                timeDown -= Time.deltaTime;
            }
            
            _coinDropAnimationFlags[coin] = false;
        }

        private Vector3 GetNextCoinDropDestinationPoint(Mob mob)
        {
            var randomAngleDegree = Random.Range(0f, 180f);
            var randomAngleRadians = Mathf.Deg2Rad * randomAngleDegree;
            var randomDirectionPivot = new Vector3(1f, 0f, -1f);
            var randomDirection =
                MathHelper.RotateVectorAboutPoint(randomDirectionPivot, Vector3.zero, Vector3.up, randomAngleRadians);
            var randomLength = Random.Range(0.45f, 1.35f);
            var randomPos = mob.transform.position + randomDirection.normalized * randomLength;
            return randomPos;
        }

        private BigInteger GetSumOfCoinsRewardForMob(Mob mob)
        {
            var currentIslandPrototype = _islandSpawner.CurrentIsland.Prototype;
            var mobPrototype = mob.Prototype;
            return currentIslandPrototype.MobsCoinsReward *
                (BigInteger)(mobPrototype.CoinsCoinsRewardMultiplier * 100f) / 100;
        }

        public float GetSpawnTime(Coin coin)
        {
            return _coinSpawnTime[coin];
        }

        public bool IsCoinDropAnimationPlaying(Coin coin)
        {
            return _coinDropAnimationFlags[coin];
        }

        private Coin Spawn(BigInteger value)
        {
            var instance = _coinPool.Spawn();
            instance.SetValue(value);
            _coins.Add(instance);
            _coinSpawnTime.Add(instance, Time.time);
            _coinCancellationTokens.Add(instance, new CancellationTokenSource());
            _coinDropAnimationFlags.Add(instance, false);
            return instance;
        }
        
        public void Despawn(Coin coin)
        {
            _coins.Remove(coin);
            _coinSpawnTime.Remove(coin);
            _coinCancellationTokens[coin].Cancel();
            _coinCancellationTokens.Remove(coin);
            _coinDropAnimationFlags.Remove(coin);
            _coinPool.Despawn(coin);
        }
    }
}