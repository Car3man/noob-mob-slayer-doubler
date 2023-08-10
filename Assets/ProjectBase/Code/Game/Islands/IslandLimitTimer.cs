using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Mobs;
using UnityEngine;

namespace Game.Islands
{
    public class IslandLimitTimer
    {
        private readonly IslandChanger _islandChanger;
        private readonly MobSpawner _mobSpawner;
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsActive => _cancellationTokenSource != null;
        public float TimeDown { get; private set; }
        public float TimeLimit => _islandChanger?.CurrentIsland.Prototype.TimeLimit ?? 0f;

        public IslandLimitTimer(
            IslandChanger islandChanger,
            MobSpawner mobSpawner
            )
        {
            _islandChanger = islandChanger;
            _mobSpawner = mobSpawner;
            
            _islandChanger.OnIslandChange += OnIslandChange;
            _mobSpawner.OnMobSpawn += OnMobSpawn;
        }

        private void OnIslandChange()
        {
            CheckTimerToStartOrStop();
        }

        private void OnMobSpawn(Mob mob)
        {
            CheckTimerToStartOrStop();
        }

        private void CheckTimerToStartOrStop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }

            var currentIslandPrototype = _islandChanger.CurrentIsland.Prototype;
            if (currentIslandPrototype.IsTimeLimit)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                UniTask.Create(() => LimitTimer(_cancellationTokenSource.Token));
            }
        }

        private async UniTask LimitTimer(CancellationToken cancellationToken)
        {
            var currentIslandPrototype = _islandChanger.CurrentIsland.Prototype;
            
            TimeDown = currentIslandPrototype.TimeLimit;
            while (TimeDown > 0)
            {
                await UniTask.DelayFrame(1, PlayerLoopTiming.Update, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                TimeDown -= Time.deltaTime;
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                TimeDown = 0f;
                _mobSpawner.SpawnNext();
            }
        }
    }
}