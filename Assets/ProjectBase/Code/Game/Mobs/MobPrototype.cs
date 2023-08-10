using Utility;

namespace Game.Mobs
{
    public class MobPrototype
    {
        public readonly string Id;
        public readonly string BaseResId;
        public readonly float? BaseHealthMultiplier;
        public readonly float? BaseCoinsCoinsRewardMultiplier;

        public MobPrototype Parent { get; private set; }
        
        public string ResId =>
            PrototypeExtensions.GetPropertyRecursive(this,prototype => prototype.Parent,
                prototype => prototype.BaseResId,property => property);

        public float HealthMultiplier =>
            PrototypeExtensions.GetPropertyRecursive(this,
                prototype => prototype.Parent,
                prototype => prototype.BaseHealthMultiplier,
                property => property.GetValueOrDefault());
        
        public float CoinsCoinsRewardMultiplier =>
            PrototypeExtensions.GetPropertyRecursive(this,
                prototype => prototype.Parent,
                prototype => prototype.BaseCoinsCoinsRewardMultiplier,
                property => property.GetValueOrDefault());

        public MobPrototype()
        {
            Id = string.Empty;
            BaseResId = string.Empty;
            BaseHealthMultiplier = 0f;
            BaseCoinsCoinsRewardMultiplier = 0f;
        }

        public MobPrototype(
            string id,
            string baseResId,
            float? baseHealthMultiplier,
            float? baseCoinsRewardMultiplier
            )
        {
            Id = id;
            BaseResId = baseResId;
            BaseHealthMultiplier = baseHealthMultiplier;
            BaseCoinsCoinsRewardMultiplier = baseCoinsRewardMultiplier;
        }

        public void SetParent(MobPrototype parent)
        {
            Parent = parent;
        }
    }
}