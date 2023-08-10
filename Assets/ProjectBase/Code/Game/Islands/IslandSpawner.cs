using Game.Configs;
using UnityEngine;

namespace Game.Islands
{
    public class IslandSpawner
    {
        private readonly IConfigProvider _configProvider;
        private readonly IslandFactory _islandFactory;
        
        public Island CurrentIsland { get; private set; }
        
        public delegate void IslandSpawnDelegate(Island island);
        public event IslandSpawnDelegate OnIslandSpawn;

        public IslandSpawner(
            IConfigProvider configProvider,
            IslandFactory islandFactory
            )
        {
            _configProvider = configProvider;
            _islandFactory = islandFactory;
        }

        public void SpawnIsland(int number)
        {
            var prototype = _configProvider.GetIslandByNumber(number);
            CurrentIsland = _islandFactory.Create(prototype);
            OnIslandSpawn?.Invoke(CurrentIsland);
        }

        public void DespawnIsland()
        {
            Object.Destroy(CurrentIsland.gameObject);
            CurrentIsland = null;
        } 
    }
}