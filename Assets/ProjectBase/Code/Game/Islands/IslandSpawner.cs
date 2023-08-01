using Game.Configs;
using Game.Saves;
using UnityEngine;

namespace Game.Islands
{
    public class IslandSpawner
    {
        private readonly IConfigProvider _configProvider;
        private readonly IslandFactory _islandFactory;
        private readonly GameSaves _gameSaves;
        
        public Island CurrentIsland { get; private set; }

        public IslandSpawner(
            IConfigProvider configProvider,
            IslandFactory islandFactory,
            GameSaves gameSaves
            )
        {
            _configProvider = configProvider;
            _islandFactory = islandFactory;
            _gameSaves = gameSaves;
        }
        
        public void RestoreFromSave()
        {
            var lastSavedIsland = _gameSaves.GetSelectedIslandId(0);
            ChangeIsland(lastSavedIsland);
        }
        
        private void ChangeAndSaveIsland(int islandId)
        {
            ChangeIsland(islandId);
            _gameSaves.SaveSelectedIslandId(islandId);
        }
        
        private void ChangeIsland(int islandId)
        {
            if (CurrentIsland != null)
            {
                DespawnIsland(CurrentIsland);
            }
            SpawnIsland(islandId);
        }

        private void SpawnIsland(int id)
        {
            var prototype = _configProvider.GetIslandById(id);
            CurrentIsland = _islandFactory.Create(prototype);
        }

        private void DespawnIsland(Island island)
        {
            Object.Destroy(island.gameObject);
        } 
    }
}