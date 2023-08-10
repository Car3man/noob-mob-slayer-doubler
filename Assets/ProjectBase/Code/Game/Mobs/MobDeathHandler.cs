using System.Numerics;
using Cysharp.Threading.Tasks;
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
            var mob = _mobSpawner.CurrentMob;
            var mobTrans = mob.transform;
            mobTrans.RotateAround(mobTrans.position, Vector3.forward, 90f);
            await UniTask.WaitForSeconds(0.5f);
        }
    }
}