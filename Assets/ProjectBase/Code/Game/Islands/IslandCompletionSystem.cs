using Game.Configs;
using Game.Mobs;
using Game.Saves;

namespace Game.Islands
{
    public class IslandCompletionSystem
    {
        private readonly GameSaves _gameSaves;
        private readonly IConfigProvider _configProvider;
        private readonly IslandSpawner _islandSpawner;

        public delegate void IslandCompleteProgressDelegate(int islandNumber, int currValue, int valueToReach);
        public event IslandCompleteProgressDelegate OnIslandCompleteProgress;
        
        public delegate void IslandCompleteDelegate(int islandNumber);
        public event IslandCompleteDelegate OnIslandComplete;

        public IslandCompletionSystem(
            GameSaves gameSaves,
            IConfigProvider configProvider,
            IslandSpawner islandSpawner,
            MobDeathHandler mobDeathHandler
            )
        {
            _gameSaves = gameSaves;
            _configProvider = configProvider;
            _islandSpawner = islandSpawner;
            mobDeathHandler.OnMobDeath += OnMobDeath;
        }

        public void ForceUnlock(int islandNumber)
        {
            var (currProgress, progressToReach) = GetProgressByIslandNumber(islandNumber);
            _gameSaves.SaveIslandKilledMobs(islandNumber, progressToReach);
            
            OnIslandCompleteProgress?.Invoke(islandNumber, progressToReach, progressToReach);
            if (currProgress < progressToReach)
            {
                OnIslandComplete?.Invoke(islandNumber);
            }
        }
        
        public bool IsUnlockedOrCompleted(int islandNumber) =>
            IsUnlocked(islandNumber) || IsCompleted(islandNumber);

        public bool IsUnlocked(int islandNumber) =>
            islandNumber == 1 || IsCompleted(islandNumber - 1);

        public bool IsCompleted(int islandNumber)
        {
            var (currProgress, progressToReach) = GetProgressByIslandNumber(islandNumber);
            return currProgress >= progressToReach;
        }

        public (int, int) GetProgressByIslandNumber(int islandNumber)
        {
            return (_gameSaves.GetIslandKilledMobs(islandNumber, 0),
                _configProvider.GetIslandByNumber(islandNumber).CountMobsToComplete);
        }

        private void OnMobDeath(Mob mob)
        {
            var currentIsland = _islandSpawner.CurrentIsland;
            var currentIslandPrototype = currentIsland.Prototype;
            
            var prevKilledMobs = _gameSaves.GetIslandKilledMobs(currentIslandPrototype.Number, 0);
            var newKilledMobs = prevKilledMobs + 1;
            var killedMobsTarget = currentIslandPrototype.CountMobsToComplete;
            
            _gameSaves.SaveIslandKilledMobs(currentIslandPrototype.Number, newKilledMobs);
            
            OnIslandCompleteProgress?.Invoke(currentIslandPrototype.Number, newKilledMobs, currentIslandPrototype.CountMobsToComplete);
            if (prevKilledMobs < killedMobsTarget && newKilledMobs >= killedMobsTarget)
            {
                OnIslandComplete?.Invoke(currentIslandPrototype.Number);
            }
        }
    }
}