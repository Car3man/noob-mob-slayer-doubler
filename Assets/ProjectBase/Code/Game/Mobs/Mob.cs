using System.Numerics;
using UnityEngine;
using Utility;

namespace Game.Mobs
{
    public class Mob : MonoBehaviour
    {
        public MobHitEffect mobHitEffect;
        
        private MobPrototype _prototype;
        private BigInteger _health;
        private BigInteger _maxHealth;

        public MobPrototype Prototype => _prototype;
        public BigInteger Health => _health;
        public BigInteger MaxHealth => _maxHealth;

        public delegate void HealthChangeDelegate(Mob mob, BigInteger prevHealth);
        public event HealthChangeDelegate OnHealthChange;

        [Zenject.Inject]
        public void Construct(MobPrototype prototype)
        {
            _prototype = prototype;
        }
        
        public void ChangeHealth(BigInteger healthDelta)
        {
            SetHealth(_health + healthDelta);
        }

        public void SetHealth(BigInteger health)
        {
            var prevHealth = _health;
            _health = BigIntegerX.Clamp(health, 0, _maxHealth);
            OnHealthChange?.Invoke(this, prevHealth);
        }
        
        public void SetMaxHealth(BigInteger maxHealth)
        {
            _maxHealth = maxHealth;
            SetHealth(_maxHealth);
        }
    }
}