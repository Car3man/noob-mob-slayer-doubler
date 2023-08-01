using System;
using Game.Mobs;
using Utility;

namespace Game.Islands
{
    public class IslandPrototype
    {
        public readonly int Id;
        public readonly MobPrototype[] BaseMobs;
        public IslandPrototype Parent { get; private set; }
        
        public MobPrototype[] Mobs =>
            PrototypeExtensions.GetPropertyRecursive(this,
                prototype => prototype.Parent,
                prototype => prototype.BaseMobs,
                property => property ?? Array.Empty<MobPrototype>());
        
        public IslandPrototype()
        {
            Id = 0;
            BaseMobs = Array.Empty<MobPrototype>();
        }

        public IslandPrototype(int id, MobPrototype[] baseMobs)
        {
            Id = id;
            BaseMobs = baseMobs;
        }

        public void SetParent(IslandPrototype parent)
        {
            Parent = parent;
        }
    }
}