using System;
using UnityEngine;

namespace Gameplay.DamageProcessing
{
    public interface IDamageable
    {
        public float CollisionDamage { get; }
        public bool IsAlive { get; }
        public Transform TargetToAim { get; }
        public void TakeDamage(float damage);
    }
}