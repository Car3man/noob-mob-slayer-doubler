using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Services;

namespace Game.Saves
{
    public class GameSaves
    {
        private readonly ISavingManager _savingManager;
        private readonly Dictionary<string, object> _cachedVariables = new();
        
        private const string CoinsKey = "coins";
        private const string UpgradeLevelKey = "upgrade_level_{0}";
        private const string SelectedIslandIdKey = "selected_island_id";

        public GameSaves(ISavingManager savingManager)
        {
            _savingManager = savingManager;
        }

        public async Task LoadAllAsync()
        {
            await _savingManager.LoadAllAsync();
            await LoadCoins();
            await LoadSelectedIslandId();
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