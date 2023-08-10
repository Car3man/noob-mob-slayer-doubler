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
        MobPrototype GetMobById(string id);
        IEnumerable<IslandPrototype> GetIslands();
        IslandPrototype GetIslandById(string id);
        IslandPrototype GetIslandByNumber(int number);
        IEnumerable<UpgradePrototype> GetUpgrades();
        UpgradePrototype GetUpgradeById(int id);
    }
}