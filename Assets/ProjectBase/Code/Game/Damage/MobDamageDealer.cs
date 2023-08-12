using System.Numerics;
using Game.Mobs;

namespace Game.Damage
{
    public class MobDamageDealer
    {
        private readonly MobSpawner _mobSpawner;

        private const float HitEffectDuration = 0.1f;

        public MobDamageDealer(MobSpawner mobSpawner)
        {
            _mobSpawner = mobSpawner;
        }

        public void DealDamage(BigInteger damage, bool playHitEffect)
        {
            var currentMob = _mobSpawner.CurrentMob;
            if (currentMob != null)
            {
                if (currentMob.Health <= 0)
                {
                    return;
                }
                
                currentMob.ChangeHealth(-damage);

                if (playHitEffect)
                {
                    currentMob.mobHitEffect.PlayHitEffect(HitEffectDuration);
                }
            }
        }
    }
}