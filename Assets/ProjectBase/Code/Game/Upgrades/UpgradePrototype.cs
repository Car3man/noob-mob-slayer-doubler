using System.Numerics;

namespace Game.Upgrades
{
    public class UpgradePrototype
    {
        public readonly int Id;
        public readonly UpgradeType Type;
        public readonly int StartLevel;
        public readonly BigInteger BaseDamage;
        public readonly BigInteger BasePrice;
        public readonly float PriceMultiplier;

        public UpgradePrototype(int id, UpgradeType type, int startLevel, BigInteger baseDamage, BigInteger basePrice, float priceMultiplier)
        {
            Id = id;
            Type = type;
            StartLevel = startLevel;
            BaseDamage = baseDamage;
            BasePrice = basePrice;
            PriceMultiplier = priceMultiplier;
        }
    }
}