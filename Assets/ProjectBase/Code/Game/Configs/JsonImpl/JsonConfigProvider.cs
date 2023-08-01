using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Islands;
using Game.Mobs;
using Game.Upgrades;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Configs.JsonImpl
{
    public class JsonConfigProvider : IConfigProvider
    {
        private readonly Dictionary<int, MobJsonObject> _mobJsonObjects = new();
        private readonly Dictionary<int, MobPrototype> _mobPrototypes = new();
        private readonly Dictionary<int, IslandJsonObject> _islandJsonObjects = new();
        private readonly Dictionary<int, IslandPrototype> _islandPrototypes = new();
        private readonly Dictionary<int, UpgradeJsonObject> _upgradeJsonObjects = new();
        private readonly Dictionary<int, UpgradePrototype> _upgradePrototypes = new();
        private bool _configsLoaded;
        
        private const string MobsConfigsPath = "Configs/Mobs/";
        private const string IslandsConfigsPath = "Configs/Islands/";
        private const string UpgradesConfigsPath = "Configs/Upgrades/";
        
        public JsonConfigProvider()
        {
        }

        public async Task LoadAllAsync()
        {
            Assert.IsFalse(_configsLoaded, "Configs already were loaded.");
            
            LoadMobs();
            LoadIslands();
            LoadUpgrades();
            
            _configsLoaded = true;

            await Task.Yield();
        }

        private void LoadMobs()
        {
            var mobConfigs = LoadConfigFiles(MobsConfigsPath);
            
            foreach (var mobConfig in mobConfigs)
            {
                var mobJsonObject = JsonConvert.DeserializeObject<MobJsonObject>(mobConfig.Text);
                _mobJsonObjects.Add(mobJsonObject.Id, mobJsonObject);
            }
            
            foreach (var mobId in _mobJsonObjects.Keys)
            {
                var mobJsonObject = _mobJsonObjects[mobId];
                var mobPrototype = new MobPrototype(
                    mobJsonObject.Id,
                    mobJsonObject.BaseHealth
                );
                _mobPrototypes.Add(mobId, mobPrototype);
            }

            foreach (var mobId in _mobPrototypes.Keys)
            {
                var parentMobId = _mobJsonObjects[mobId].ParentId;
                if (!parentMobId.HasValue)
                {
                    continue;
                }
                
                var mobPrototype = _mobPrototypes[mobId];
                var parentMobPrototype = _mobPrototypes[parentMobId.Value];
                mobPrototype.SetParent(parentMobPrototype);
            }
        }

        private void LoadIslands()
        {
            var islandConfigs = LoadConfigFiles(IslandsConfigsPath);

            foreach (var islandConfig in islandConfigs)
            {
                var islandJsonObject = JsonConvert.DeserializeObject<IslandJsonObject>(islandConfig.Text);
                _islandJsonObjects.Add(islandJsonObject.Id, islandJsonObject);
            }
            
            foreach (var islandId in _islandJsonObjects.Keys)
            {
                var islandJsonObject = _islandJsonObjects[islandId];
                var islandPrototype = new IslandPrototype(
                    islandJsonObject.Id,
                    islandJsonObject.MobsIds?.Select(mobId => _mobPrototypes[mobId]).ToArray()
                );
                _islandPrototypes.Add(islandId, islandPrototype);
            }
            
            foreach (var islandId in _islandPrototypes.Keys)
            {
                var parentIslandId = _islandJsonObjects[islandId].ParentId;
                if (!parentIslandId.HasValue)
                {
                    continue;
                }
                
                var islandPrototype = _islandPrototypes[islandId];
                var parentIslandPrototype = _islandPrototypes[parentIslandId.Value];
                islandPrototype.SetParent(parentIslandPrototype);
            }
        }

        private void LoadUpgrades()
        {
            var upgradeConfigs = LoadConfigFiles(UpgradesConfigsPath);

            foreach (var upgradeConfig in upgradeConfigs)
            {
                var upgradeJsonObject = JsonConvert.DeserializeObject<UpgradeJsonObject>(upgradeConfig.Text);
                _upgradeJsonObjects.Add(upgradeJsonObject.Id, upgradeJsonObject);
            }
            
            foreach (var upgradeId in _upgradeJsonObjects.Keys)
            {
                var upgradeJsonObject = _upgradeJsonObjects[upgradeId];
                var upgradePrototype = new UpgradePrototype(
                    upgradeId,
                    System.Enum.Parse<UpgradeType>(upgradeJsonObject.Type),
                    upgradeJsonObject.StartLevel,
                    upgradeJsonObject.BaseDamage,
                    upgradeJsonObject.BasePrice,
                    upgradeJsonObject.PriceMultiplier
                );
                _upgradePrototypes.Add(upgradeId, upgradePrototype);
            }
        }

        public IEnumerable<MobPrototype> GetMobs()
        {
            ThrowIfConfigsNotLoaded();
            
            return _mobPrototypes.Select(x => x.Value).AsEnumerable();
        }

        public MobPrototype GetMobById(int id)
        {
            ThrowIfConfigsNotLoaded();

            return _mobPrototypes[id];
        }
        
        public IEnumerable<IslandPrototype> GetIslands()
        {
            ThrowIfConfigsNotLoaded();
            
            return _islandPrototypes.Select(x => x.Value).AsEnumerable();
        }

        public IslandPrototype GetIslandById(int id)
        {
            ThrowIfConfigsNotLoaded();

            return _islandPrototypes[id];
        }

        public IEnumerable<UpgradePrototype> GetUpgrades()
        {
            ThrowIfConfigsNotLoaded();
            
            return _upgradePrototypes.Select(x => x.Value).AsEnumerable();
        }

        public UpgradePrototype GetUpgradeById(int id)
        {
            ThrowIfConfigsNotLoaded();

            return _upgradePrototypes[id];
        }

        private void ThrowIfConfigsNotLoaded()
        {
            Assert.IsTrue(_configsLoaded, "Load configs before get them.");
        }

        private IEnumerable<JsonConfigFileInfo> LoadConfigFiles(string path)
        {
            var textAssets = Resources.LoadAll<TextAsset>(path);
            return textAssets
                .Select(textAsset => new JsonConfigFileInfo(textAsset.name, textAsset.text));
        }

        private async UniTask<JsonConfigFileInfo> LoadConfigFileAsync(string path)
        {
            var textAsset = (TextAsset)await Resources.LoadAsync<TextAsset>(path);
            return new JsonConfigFileInfo(textAsset.name, textAsset.text);
        }
    }
}