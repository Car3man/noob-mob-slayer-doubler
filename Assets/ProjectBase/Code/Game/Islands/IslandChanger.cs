using Game.Mobs;
using Game.Saves;

namespace Game.Islands
{
    public class IslandChanger
    {
        private readonly IslandSpawner _islandSpawner;
        private readonly IslandCompletionSystem _islandCompletionSystem;
        private readonly GameSaves _gameSaves;
        private readonly MobSpawner _mobSpawner;

        public Island CurrentIsland => _islandSpawner.CurrentIsland;
        
        public delegate void IslandChangeDelegate();
        public event IslandChangeDelegate OnIslandChange;

        public IslandChanger(
            GameSaves gameSaves,
            IslandSpawner islandSpawner,
            IslandCompletionSystem islandCompletionSystem,
            MobSpawner mobSpawner
        )
        {
            _gameSaves = gameSaves;
            _islandSpawner = islandSpawner;
            _islandCompletionSystem = islandCompletionSystem;
            _mobSpawner = mobSpawner;
        }
        
        public void RestoreFromSave()
        {
            var lastSavedIsland = _gameSaves.GetSelectedIslandId(1);
            ChangeIsland(lastSavedIsland);
        }
        
        public void ChangeAndSaveIsland(int islandNumber)
        {
            if (!_islandCompletionSystem.IsUnlockedOrCompleted(islandNumber))
            {
                return;
            }
            
            ChangeIsland(islandNumber);
            _gameSaves.SaveSelectedIslandId(islandNumber);
        }
        
        private void ChangeIsland(int islandNumber)
        {
            if (CurrentIsland != null)
            {
                _islandSpawner.DespawnIsland();
            }
            
            _islandSpawner.SpawnIsland(islandNumber);
            _mobSpawner.SpawnNext();
            
            OnIslandChange?.Invoke();
        }
    }
}