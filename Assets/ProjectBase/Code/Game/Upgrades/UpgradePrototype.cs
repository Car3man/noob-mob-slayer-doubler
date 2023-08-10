using System.Numerics;

namespace Game.Upgrades
{
    public class UpgradePrototype
    {
        public readonly int Id;
        public readonly UpgradeType Type;
        public readonly int StartLevel;
        public readonly BigInteger BaseBaseDamage;
        public readonly BigInteger BaseBasePrice;
        public readonly float BasePriceMultiplier;
        public readonly int MaxLevel;

        public UpgradePrototype(
            int id,
            UpgradeType type,
            int startLevel,
            BigInteger baseBaseDamage,
            BigInteger baseBasePrice,
            float basePriceMultiplier,
            int maxLevel
            )
        {
            Id = id;
            Type = type;
            StartLevel = startLevel;
            BaseBaseDamage = baseBaseDamage;
            BaseBasePrice = baseBasePrice;
            BasePriceMultiplier = basePriceMultiplier;
            MaxLevel = maxLevel;
        }
    }
}