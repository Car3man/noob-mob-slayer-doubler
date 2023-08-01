using System.Numerics;
using Utility;

namespace Game.Mobs
{
    public class MobPrototype
    {
        public readonly int Id;
        public readonly BigInteger? BaseHealth;

        public MobPrototype Parent { get; private set; }

        public BigInteger Health =>
            PrototypeExtensions.GetPropertyRecursive(this,
                prototype => prototype.Parent,
                prototype => prototype.BaseHealth,
                property => property.GetValueOrDefault());

        public MobPrototype()
        {
            Id = 0;
            BaseHealth = 0;
        }

        public MobPrototype(
            int id,
            BigInteger? baseHealth
            )
        {
            Id = id;
            BaseHealth = baseHealth;
        }

        public void SetParent(MobPrototype parent)
        {
            Parent = parent;
        }
    }
}