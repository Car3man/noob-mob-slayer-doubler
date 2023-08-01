using System.Numerics;
using UnityEngine;

namespace Game.Upgrades
{
    public class Upgrade
    {
        private readonly UpgradePrototype _prototype;

        public int Id => _prototype.Id;
        public UpgradeType Type => _prototype.Type;
        public int StartLevel => _prototype.StartLevel;
        public int Level { get; private set; }

        public Upgrade(UpgradePrototype prototype)
        {
            _prototype = prototype;
            
            SetLevel(prototype.StartLevel);
        }

        public BigInteger GetDamage() =>
            _prototype.BaseDamage * Level;

        public BigInteger GetUpgradePrice()
        {
            float multiplier = Mathf.Pow(_prototype.PriceMultiplier, Level + 1);
            BigInteger bigMultiplier = (BigInteger)(multiplier * 100f);
            return _prototype.BasePrice * bigMultiplier / 100;
        }

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}