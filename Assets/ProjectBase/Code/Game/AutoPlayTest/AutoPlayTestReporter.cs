using System.Collections.Generic;
using Game.Configs;
using Game.Islands;
using Infrastructure.SceneStarters;
using UnityEngine;
using Zenject;

namespace Game.AutoPlayTest
{
    public class AutoPlayTestReporter : ITickable
    {
        private readonly GameSceneStarter _gameSceneStarter;
        private readonly IConfigProvider _configProvider;
        private readonly IslandChanger _islandChanger;
        
        private Dictionary<int, float> _islandCompleteTime;

        public AutoPlayTestReporter(
            GameSceneStarter gameSceneStarter,
            IConfigProvider configProvider,
            IslandChanger islandChanger,
            IslandCompletionSystem islandCompletionSystem
            )
        {
            _gameSceneStarter = gameSceneStarter;
            _configProvider = configProvider;
            _islandChanger = islandChanger;
            
            islandCompletionSystem.OnIslandComplete += OnIslandComplete;
        }

        public void Tick()
        {
            if (!_gameSceneStarter.Started)
            {
                return;
            }
            
            if (_islandCompleteTime == null)
            {
                _islandCompleteTime = new Dictionary<int, float>();
                
                foreach (var islandPrototype in _configProvider.GetIslands())
                {
                    _islandCompleteTime.Add(islandPrototype.Number, 0f);
                }
            }
            
            var currIsland = _islandChanger.CurrentIsland;
            if (currIsland == null)
            {
                return;
            }

            _islandCompleteTime[currIsland.Prototype.Number] += Time.deltaTime;
        }

        private void OnIslandComplete(int islandNumber)
        {
            Debug.Log($"Island {islandNumber} completed for {_islandCompleteTime[islandNumber]} seconds.");

            if (islandNumber % 10 == 0)
            {
                var completedIsland = _configProvider.GetIslandByNumber(islandNumber);
                var worldCompleteTime = 0f;
                for (int i = islandNumber; i > islandNumber - 10; i--)
                {
                    worldCompleteTime += _islandCompleteTime[i];
                }
                Debug.Log($"<color=#00FF00>World {completedIsland.ResId} completed for {worldCompleteTime} seconds.</color>");
            }
        }
    }
}