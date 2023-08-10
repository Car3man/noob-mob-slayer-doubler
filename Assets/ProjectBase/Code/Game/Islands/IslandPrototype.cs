using System;
using System.Numerics;
using Game.Mobs;
using Utility;

namespace Game.Islands
{
    public class IslandPrototype
    {
        public readonly string Id;
        public readonly int Number;
        public readonly string BaseResId;
        public readonly MobPrototype[] BaseMobs;
        public readonly int? BaseCountMobsToComplete;
        public readonly BigInteger? BaseMobsHealth;
        public readonly BigInteger? BaseMobsCoinsReward;
        public readonly bool? BaseIsTimeLimit;
        public readonly float? BaseTimeLimit;
        public IslandPrototype Parent { get; private set; }
        
        public string ResId =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseResId,property => property);
        
        public MobPrototype[] Mobs =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseMobs,property => property ?? Array.Empty<MobPrototype>());
        
        public int CountMobsToComplete =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseCountMobsToComplete,property => property.GetValueOrDefault());
        
        public BigInteger MobsHealth =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseMobsHealth,property => property.GetValueOrDefault());
        
        public BigInteger MobsCoinsReward =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseMobsCoinsReward,property => property.GetValueOrDefault());
        
        public bool IsTimeLimit =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseIsTimeLimit,property => property.GetValueOrDefault());
        
        public float TimeLimit =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseTimeLimit,property => property.GetValueOrDefault());
        
        public IslandPrototype()
        {
            Id = string.Empty;
            Number = 0;
            BaseResId = string.Empty;
            BaseMobs = Array.Empty<MobPrototype>();
            BaseCountMobsToComplete = 0;
            BaseMobsHealth = 0;
            BaseMobsCoinsReward = 0;
            BaseIsTimeLimit = false;
            BaseTimeLimit = 0f;
        }

        public IslandPrototype(
            string id,
            int number,
            string baseResId,
            MobPrototype[] baseMobs,
            int? baseCountMobsToComplete,
            BigInteger? baseMobsHealth,
            BigInteger? baseMobsCoinsReward,
            bool? baseIsTimeLimit,
            float? baseTimeLimit
            )
        {
            Id = id;
            Number = number;
            BaseResId = baseResId;
            BaseMobs = baseMobs;
            BaseCountMobsToComplete = baseCountMobsToComplete;
            BaseMobsHealth = baseMobsHealth;
            BaseMobsCoinsReward = baseMobsCoinsReward;
            BaseIsTimeLimit = baseIsTimeLimit;
            BaseTimeLimit = baseTimeLimit;
        }

        public void SetParent(IslandPrototype parent)
        {
            Parent = parent;
        }
    }
}