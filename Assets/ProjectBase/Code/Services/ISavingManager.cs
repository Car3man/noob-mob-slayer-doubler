using System.Threading.Tasks;

namespace Services
{
    public interface ISavingManager
    {
        Task<bool> IsExist(string key);
        Task<string> LoadStringAsync(string key, string defaultValue);
        Task<int> LoadIntAsync(string key, int defaultValue);
        Task<float> LoadFloatAsync(string key, float defaultValue);
        Task<bool> LoadBoolAsync(string key, bool defaultValue);
        Task SaveStringAsync(string key, string value);
        Task SaveIntAsync(string key, int value);
        Task SaveFloatAsync(string key, float value);
        Task SaveBoolAsync(string key, bool value);
        Task LoadAllAsync();
        Task SaveAllAsync();
    }
}