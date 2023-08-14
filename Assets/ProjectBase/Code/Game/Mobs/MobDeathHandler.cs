using System.Numerics;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Game.Mobs
{
    public class MobDeathHandler
    {
        private readonly IAudioManager _audioManager;
        private readonly MobSpawner _mobSpawner;
        
        public delegate void MobDeathDelegate(Mob mob);
        public event MobDeathDelegate OnMobDeath;

        public MobDeathHandler(
            IAudioManager audioManager,
            MobSpawner mobSpawner
            )
        {
            _audioManager = audioManager;
            _mobSpawner = mobSpawner;
            _mobSpawner.OnMobSpawn += OnMobSpawn;
            _mobSpawner.OnMobDespawn += OnMobDespawn;
        }

        private void OnMobSpawn(Mob mob)
        {
            mob.OnHealthChange += OnMobHealthChange;
        }

        private void OnMobDespawn(Mob mob)
        {
            mob.OnHealthChange -= OnMobHealthChange;
        }
        
        private async void OnMobHealthChange(Mob mob, BigInteger prevHealth)
        {
            if (prevHealth > 0 && mob.Health <= 0)
            {
                OnMobDeath?.Invoke(mob);
                MobDeathSound(mob);
                await MobDeathAnimation(mob);
                _mobSpawner.SpawnNext();
            }
        }

        private void MobDeathSound(Mob mob)
        {
            _audioManager.PlaySound($"MobDeaths/{mob.Prototype.ResId}");
        }

        private async UniTask MobDeathAnimation(Mob mob)
        {
            const float rotateDuration = 0.3f;
            const float delayDuration = 0.2f;
            
            var mobTrans = mob.transform;
            var timeDown = rotateDuration;
            while (timeDown > 0f)
            {
                mobTrans.eulerAngles = Vector3.zero;
                var angle = (1f - timeDown / rotateDuration) * 90f;
                mobTrans.RotateAround(mobTrans.position, Vector3.right, angle);
                await UniTask.WaitForEndOfFrame(mob);
                timeDown -= Time.deltaTime;
            }
            await UniTask.WaitForSeconds(delayDuration);
        }
    }
}