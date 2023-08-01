using System.Numerics;
using UnityEngine;
using Utility;

namespace Game.Mobs
{
    public class Mob : MonoBehaviour
    {
        private MobPrototype _prototype;
        private BigInteger _health;
        
        public int Id => _prototype.Id;
        public BigInteger Health => _health;
        public BigInteger MaxHealth => _prototype.Health;

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
            _health = BigIntegerX.Clamp(health, 0, MaxHealth);
            OnHealthChange?.Invoke(this, prevHealth);
        }
    }
}