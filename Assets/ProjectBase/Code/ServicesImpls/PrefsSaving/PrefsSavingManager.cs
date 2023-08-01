using System.Threading.Tasks;
using Services;
using UnityEngine;

namespace ServicesImpls.PrefsSaving
{
    public class PrefsSavingManager : ISavingManager
    {
        public Task<bool> IsExist(string key)
        {
            return Task.FromResult(PlayerPrefs.HasKey(key));
        }

        public Task<string> LoadStringAsync(string key, string defaultValue)
        {
            var stringResult = PlayerPrefs.GetString(key, defaultValue);
            return Task.FromResult(stringResult);
        }

        public Task<int> LoadIntAsync(string key, int defaultValue)
        {
            var intResult = PlayerPrefs.GetInt(key, defaultValue);
            return Task.FromResult(intResult);
        }

        public Task<float> LoadFloatAsync(string key, float defaultValue)
        {
            var floatResult = PlayerPrefs.GetFloat(key, defaultValue);
            return Task.FromResult(floatResult);
        }

        public Task<bool> LoadBoolAsync(string key, bool defaultValue)
        {
            var boolResult = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
            return Task.FromResult(boolResult);
        }

        public Task SaveStringAsync(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }

        public Task SaveIntAsync(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }

        public Task SaveFloatAsync(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }

        public Task SaveBoolAsync(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }

        public Task LoadAllAsync()
        {
            return Task.CompletedTask;
        }

        public Task SaveAllAsync()
        {
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }
    }
}