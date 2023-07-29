using System;
using _ScriptableObjects.Classes;
using UnityEngine;

namespace Gameplay.DamageProcessing
{
    public class DamageTaker : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform _aimTarget;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Collider _damageCollider;
        [SerializeField] private bool _takeDamageFromCollisions;

        private float _maxHealth;
        private float _currentHealth;
        public Transform TargetToAim => _aimTarget;
        public float CollisionDamage { get; private set; }
        public bool IsAlive => _currentHealth > 0;
        
        public void Init(CharacterSettings settings)
        {
            _maxHealth = settings.Health;
            CollisionDamage = settings.CollisionDamage;
            _currentHealth = _maxHealth;
            _healthBar.Init(_maxHealth);
            _damageCollider.enabled = true;
        }

        public void TakeDamage(float damage)
        {
            if (IsAlive == false)
                return;

            DecreaseHealth(damage);
            ChangeView();
            CheckDeath();
        }

        private void DecreaseHealth(float damage) =>
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        private void ChangeView() => _healthBar.ChangeValue(_currentHealth, 0.65f);

        private void CheckDeath()
        {
            if (IsAlive == false)
            {
                _damageCollider.enabled = false;
                _healthBar.Hide();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_takeDamageFromCollisions == false)
                return;
            
            if (other.TryGetComponent(out IDamageable damageable))
            {
                TakeDamage(damageable.CollisionDamage);
            }
        }
    }
}