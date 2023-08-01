using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Islands;
using Game.Mobs;
using Game.Upgrades;

namespace Game.Configs
{
    public interface IConfigProvider
    {
        Task LoadAllAsync();
        IEnumerable<MobPrototype> GetMobs();
        MobPrototype GetMobById(int id);
        IEnumerable<IslandPrototype> GetIslands();
        IslandPrototype GetIslandById(int id);
        IEnumerable<UpgradePrototype> GetUpgrades();
        UpgradePrototype GetUpgradeById(int id);
    }
}