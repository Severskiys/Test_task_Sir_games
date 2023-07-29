using Gameplay.DamageProcessing;
using UnityEngine;
using Utils.Pool;

namespace _ScriptableObjects.Classes
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "ISN/CharacterSettings")]
    public class CharacterSettings : ScriptableObject
    {
        [SerializeField, Range(0,2000)] private float _hitPoints;
        [SerializeField, Range(0,20)] private int _speed;
        [SerializeField, Range(0,20)] private float _damage;
        [SerializeField, Range(0,5)] private float _delayAfterAttack;
        [SerializeField, Range(0,5)] private float _projectileFlyTime;
        [SerializeField, Range(5,20)] private float _locationRadius;
        [SerializeField, Range(0,100)] private float _collisionDamage;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private PooledParticle _hitParticlePrefab;

        public float Health => _hitPoints;
        public int Speed => _speed;
        public float Damage => _damage;
        public float DelayAfterAttack => _delayAfterAttack;
        public float ProjectileFlyTime => _projectileFlyTime;
        public Projectile ProjectilePrefab => _projectilePrefab;
        public PooledParticle HitParticlePrefab => _hitParticlePrefab;
        public float LocationRadius => _locationRadius;
        public float CollisionDamage => _collisionDamage;
    }
}