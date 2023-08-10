using System.Numerics;
using UnityEngine;

namespace Game.Upgrades
{
    public class Upgrade
    {
        private readonly UpgradePrototype _prototype;

        public UpgradePrototype Prototype => _prototype;
        public int Level { get; private set; }

        public Upgrade(UpgradePrototype prototype)
        {
            _prototype = prototype;
            
            SetLevel(prototype.StartLevel);
        }

        public BigInteger GetDamage() =>
            _prototype.BaseBaseDamage * Level;

        public BigInteger GetUpgradePrice()
        {
            float multiplier = Mathf.Pow(_prototype.BasePriceMultiplier, Level + 1);
            BigInteger bigMultiplier = (BigInteger)(multiplier * 100f);
            return _prototype.BaseBasePrice * bigMultiplier / 100;
        }

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}