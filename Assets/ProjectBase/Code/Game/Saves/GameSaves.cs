using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Game.Configs;
using Services;

namespace Game.Saves
{
    public class GameSaves
    {
        private readonly ISavingManager _savingManager;
        private readonly IConfigProvider _configProvider;
        private readonly Dictionary<string, object> _cachedVariables = new();
        
        private const string CoinsKey = "coins";
        private const string UpgradeLevelKey = "upgrade_level_{0}";
        private const string SelectedIslandIdKey = "selected_island_id";
        private const string IslandKilledMobsKey = "island_{0}_killed_mobs";
        private const string EnchantmentStatusKey = "enchantment_{0}";

        public GameSaves(
            ISavingManager savingManager,
            IConfigProvider configProvider
            )
        {
            _savingManager = savingManager;
            _configProvider = configProvider;
        }

        public async Task LoadAllAsync()
        {
            await _savingManager.LoadAllAsync();
            await LoadCoins();
            await LoadSelectedIslandId();
            foreach (var upgradePrototype in _configProvider.GetUpgrades())
            {
                await LoadUpgradeLevel(upgradePrototype.Id);
            }
            foreach (var islandPrototype in _configProvider.GetIslands())
            {
                await LoadIslandKilledMobs(islandPrototype.Number);
            }
            foreach (var enchantmentPrototype in _configProvider.GetEnchantments())
            {
                await LoadEnchantmentStatus(enchantmentPrototype.Id);
            }
        }
        
        public BigInteger GetCoins(BigInteger defaultValue) => 
            GetVariable(CoinsKey, defaultValue);
        private async Task LoadCoins() =>
            await LoadVariable<BigInteger>(CoinsKey);
        public async void SaveCoins(BigInteger coins, bool immediatelySave = true) =>
            await SaveVariable<BigInteger>(CoinsKey, coins, immediatelySave);

        public int GetSelectedIslandId(int defaultValue) =>
            GetVariable(SelectedIslandIdKey, defaultValue);
        private async Task LoadSelectedIslandId() =>
            await LoadVariable<int>(SelectedIslandIdKey);
        public async void SaveSelectedIslandId(int selectedIslandId, bool immediatelySave = true) =>
            await SaveVariable<int>(SelectedIslandIdKey, selectedIslandId, immediatelySave);

        public int GetUpgradeLevel(int upgradeId, int defaultValue) =>
            GetVariable(FormatUpgradeLevelKey(upgradeId), defaultValue);
        private async Task LoadUpgradeLevel(int upgradeId) =>
            await LoadVariable<int>(FormatUpgradeLevelKey(upgradeId));
        public async void SaveUpgradeLevel(int upgradeId, int upgradeLevel, bool immediatelySave = true) =>
            await SaveVariable<int>(FormatUpgradeLevelKey(upgradeId), upgradeLevel, immediatelySave);
        private string FormatUpgradeLevelKey(int upgradeId) =>
            string.Format(UpgradeLevelKey, upgradeId);
        
        public int GetIslandKilledMobs(int islandNumber, int defaultValue) =>
            GetVariable(FormatIslandKilledMobsKey(islandNumber), defaultValue);
        private async Task LoadIslandKilledMobs(int islandNumber) =>
            await LoadVariable<int>(FormatIslandKilledMobsKey(islandNumber));
        public async void SaveIslandKilledMobs(int islandNumber, int islandKilledMobs, bool immediatelySave = true) =>
            await SaveVariable<int>(FormatIslandKilledMobsKey(islandNumber), islandKilledMobs, immediatelySave);
        private string FormatIslandKilledMobsKey(int islandNumber) =>
            string.Format(IslandKilledMobsKey, islandNumber);
        
        public bool IsEnchantmentActive(string enchantmentId, bool defaultValue) =>
            GetVariable(FormatEnchantmentStatusKey(enchantmentId), defaultValue);
        private async Task LoadEnchantmentStatus(string enchantmentId) =>
            await LoadVariable<bool>(FormatEnchantmentStatusKey(enchantmentId));
        public async void SaveEnchantmentStatus(string enchantmentId, bool isActive, bool immediatelySave = true) =>
            await SaveVariable<bool>(FormatEnchantmentStatusKey(enchantmentId), isActive, immediatelySave);
        private string FormatEnchantmentStatusKey(string enchantmentId) =>
            string.Format(EnchantmentStatusKey, enchantmentId);

        private T GetVariable<T>(string key, T defaultValue)
        {
            if (_cachedVariables.TryGetValue(key, out var variable))
            {
                return (T)variable;
            }
            return defaultValue;
        } 

        private async Task LoadVariable<T>(string key)
        {
            if (!await _savingManager.IsExist(key))
            {
                return;
            }
            
            var variableType = typeof(T).Name;
            switch (variableType)
            {
                case nameof(String):
                    var stringVariable = await _savingManager.LoadStringAsync(key, default);
                    _cachedVariables[key] = stringVariable;
                    break;
                case nameof(Int32):
                    var intVariable = await _savingManager.LoadIntAsync(key, default);
                    _cachedVariables[key] = intVariable;
                    break;
                case nameof(Single):
                    var singleVariable = await _savingManager.LoadFloatAsync(key, default);
                    _cachedVariables[key] = singleVariable;
                    break;
                case nameof(Boolean):
                    var booleanVariable = await _savingManager.LoadBoolAsync(key, default);
                    _cachedVariables[key] = booleanVariable;
                    break;
                case nameof(BigInteger):
                    var stringBigIntegerVariable = await _savingManager.LoadStringAsync(key, default);
                    var bigIntegerVariable = BigInteger.Parse(stringBigIntegerVariable);
                    _cachedVariables[key] = bigIntegerVariable;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown type: {variableType}");
            }
        }

        private async Task SaveVariable<T>(string key, T value, bool immediatelySave)
        {
            object boxedVariable = value;
            _cachedVariables[key] = boxedVariable;
            
            if (immediatelySave)
            {
                var variableType = typeof(T).Name;
                switch (variableType)
                {
                    case nameof(String):
                        await _savingManager.SaveStringAsync(key, (string)boxedVariable);
                        break;
                    case nameof(Int32):
                        await _savingManager.SaveIntAsync(key, (int)boxedVariable);
                        break;
                    case nameof(Single):
                        await _savingManager.SaveFloatAsync(key, (float)boxedVariable);
                        break;
                    case nameof(Boolean):
                        await _savingManager.SaveBoolAsync(key, (bool)boxedVariable);
                        break;
                    case nameof(BigInteger):
                        await _savingManager.SaveStringAsync(key, ((BigInteger)boxedVariable).ToString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown type: {variableType}");
                }
            }
        }
    }
}