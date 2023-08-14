using System.Collections.Generic;
using System.Linq;
using Game.Configs;
using Game.Islands;
using Game.Saves;

namespace Game.Enchantments
{
    public class EnchantmentInventory
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameSaves _gameSaves;
        private Dictionary<string, Enchantment> _enchantments;

        public EnchantmentInventory(
            IConfigProvider configProvider,
            GameSaves gameSaves,
            IslandCompletionSystem islandCompletionSystem
            )
        {
            _configProvider = configProvider;
            _gameSaves = gameSaves;
            
            islandCompletionSystem.OnIslandComplete += OnIslandComplete;
        }
        
        public void Initialize()
        {
            _enchantments = _configProvider.GetEnchantments()
                .Select(upgradePrototype => new Enchantment(upgradePrototype))
                .ToDictionary(upgrade => upgrade.Prototype.Id, upgrade => upgrade);
        }
        
        public void RestoreFromSave()
        {
            foreach (var enchantmentId in _enchantments.Keys)
            {
                var enchantment = _enchantments[enchantmentId];
                var isActive = _gameSaves.IsEnchantmentActive(enchantmentId, false);
                enchantment.SetActive(isActive);
            }
        }

        private void OnIslandComplete(int islandNumber)
        {
            
        }

        public IEnumerable<Enchantment> GetActiveEnchantments() => _enchantments
            .Where(x => x.Value.Active)
            .Select(x => x.Value)
            .AsEnumerable();

        public IEnumerable<Enchantment> GetActiveEnchantmentsByUpgradeId(int upgradeId) => GetActiveEnchantments()
            .Where(enchantment => enchantment.Prototype.UpgradeId == upgradeId);
    }
}