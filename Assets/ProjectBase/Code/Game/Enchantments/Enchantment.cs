namespace Game.Enchantments
{
    public class Enchantment
    {
        private readonly EnchantmentPrototype _prototype;

        public EnchantmentPrototype Prototype => _prototype;

        public bool Active { get; private set; }

        public Enchantment(EnchantmentPrototype prototype)
        {
            _prototype = prototype;
        }

        public void SetActive(bool value)
        {
            Active = value;
        }
    }
}