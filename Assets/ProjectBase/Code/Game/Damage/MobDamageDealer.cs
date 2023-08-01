using System.Numerics;
using Game.Mobs;

namespace Game.Damage
{
    public class MobDamageDealer
    {
        private readonly MobSpawner _mobSpawner;

        public MobDamageDealer(MobSpawner mobSpawner)
        {
            _mobSpawner = mobSpawner;
        }

        public void DealDamage(BigInteger damage)
        {
            if (_mobSpawner.CurrentMob != null)
            {
                _mobSpawner.CurrentMob.ChangeHealth(-damage);
            }
        }
    }
}