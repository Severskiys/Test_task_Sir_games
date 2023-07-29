using _ScriptableObjects.Classes;
using UnityEngine;
using Utils.Pool;

namespace Gameplay.DamageProcessing
{
    public class AttackFactory
    {
        private readonly Projectile _projectilePrefab;
        private readonly PooledParticle _hitParticlePrefab;

        public AttackFactory(CharacterSettings settings)
        {
            _projectilePrefab = settings.ProjectilePrefab;
            _hitParticlePrefab = settings.HitParticlePrefab;
        }
        
        public Projectile GetProjectile(Transform point)
        {
            Projectile projectile = Pool.Get(_projectilePrefab);
            projectile.SetParent(point);
            projectile.SetPosition(point.position);
            return projectile;
        }

        public PooledParticle GetHitParticle(Vector3 point)
        {
            PooledParticle hitParticle = Pool.Get(_hitParticlePrefab);
            hitParticle.SetPosition(point);
            return hitParticle;
        }
    }
}