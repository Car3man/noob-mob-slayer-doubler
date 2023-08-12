using System.Numerics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Game.Mobs
{
    public class MobDeathHandler
    {
        private readonly MobSpawner _mobSpawner;
        
        public delegate void MobDeathDelegate(Mob mob);
        public event MobDeathDelegate OnMobDeath;

        public MobDeathHandler(MobSpawner mobSpawner)
        {
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
                await MobDeathAnimation();
                _mobSpawner.SpawnNext();
            }
        }
        
        private async UniTask MobDeathAnimation()
        {
            const float rotateDuration = 0.3f;
            const float delayDuration = 0.2f;
            
            var mob = _mobSpawner.CurrentMob;
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