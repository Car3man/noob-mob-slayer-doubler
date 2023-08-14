using System.Linq;
using Game.Configs;
using Game.Mobs;
using Game.Saves;

namespace Game.Islands
{
    public class IslandChanger
    {
        private readonly IslandSpawner _islandSpawner;
        private readonly IslandCompletionSystem _islandCompletionSystem;
        private readonly IConfigProvider _configProvider;
        private readonly GameSaves _gameSaves;
        private readonly MobSpawner _mobSpawner;

        public Island CurrentIsland => _islandSpawner.CurrentIsland;
        
        public delegate void IslandChangeDelegate();
        public event IslandChangeDelegate OnIslandChange;

        public IslandChanger(
            IConfigProvider configProvider,
            GameSaves gameSaves,
            IslandSpawner islandSpawner,
            IslandCompletionSystem islandCompletionSystem,
            MobSpawner mobSpawner
        )
        {
            _configProvider = configProvider;
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

        public bool IsIslandNumberValid(int islandNumber) =>
            islandNumber >= GetMinIslandNumber() && islandNumber <= GetMaxIslandNumber();
        
        public int GetMinIslandNumber() => _configProvider
            .GetIslands()
            .OrderBy(x => x.Number)
            .First()
            .Number;
        
        public int GetMaxIslandNumber() => _configProvider
            .GetIslands()
            .OrderBy(x => x.Number)
            .Last()
            .Number;
        
        public void ChangeAndSaveIsland(int islandNumber)
        {
            if (!IsIslandNumberValid(islandNumber))
            {
                return;
            }
            
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