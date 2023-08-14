namespace Game.Enchantments
{
    public class EnchantmentPrototype
    {
        public readonly string Id;
        public readonly int UpgradeId;
        public readonly float? DamageMultiplier;

        public EnchantmentPrototype(
            string id,
            int upgradeId,
            float? damageMultiplier
            )
        {
            Id = id;
            UpgradeId = upgradeId;
            DamageMultiplier = damageMultiplier;
        }
    }
}