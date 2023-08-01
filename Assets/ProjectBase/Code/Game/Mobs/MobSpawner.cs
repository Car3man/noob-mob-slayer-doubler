using Game.Configs;
using Game.Islands;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Mobs
{
    public class MobSpawner
    {
        private readonly IConfigProvider _configProvider;
        private readonly MobFactory _mobFactory;
        private readonly IslandSpawner _islandSpawner;
        
        public Mob CurrentMob { get; private set; }
        
        public delegate void MobSpawnDelegate(Mob mob);
        public event MobSpawnDelegate OnMobSpawn;

        public delegate void MobDespawnDelegate(Mob mob);
        public event MobDespawnDelegate OnMobDespawn;

        public MobSpawner(
            IConfigProvider configProvider,
            MobFactory mobFactory,
            IslandSpawner islandSpawner
            )
        {
            _configProvider = configProvider;
            _mobFactory = mobFactory;
            _islandSpawner = islandSpawner;
        }
        
        public void SpawnNext()
        {
            var currentIsland = _islandSpawner.CurrentIsland;
            var availableToSpawnMobs = currentIsland.Mobs;
            var randomMob = availableToSpawnMobs[Random.Range(0, availableToSpawnMobs.Length)];
            ChangeMob(randomMob.Id);
        }
        
        private void ChangeMob(int mobId)
        {
            if (CurrentMob != null)
            {
                DespawnMob(CurrentMob);
            }
            SpawnMob(mobId);
        }

        private void SpawnMob(int id)
        {
            Assert.IsNotNull(_islandSpawner.CurrentIsland, "Can't spawn mob without island");
            
            var spawnPoint = _islandSpawner.CurrentIsland.MobSpawnPoint;
            
            var mobPrototype = _configProvider.GetMobById(id);
            var mobInstance = _mobFactory.Create(mobPrototype);
            mobInstance.SetHealth(mobInstance.MaxHealth);
            mobInstance.transform.position = spawnPoint.transform.position;

            CurrentMob = mobInstance;
            OnMobSpawn?.Invoke(CurrentMob);
        }
        
        private void DespawnMob(Mob mob)
        {
            Object.Destroy(mob.gameObject);
        } 
    }
}