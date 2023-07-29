using System.Collections;
using _ScriptableObjects.Classes;
using UnityEngine;
using Utils.Pool;

namespace Gameplay.DamageProcessing
{
    public class Projectile : PoolItem
    {
        [SerializeField] private ParticleSystem _trail;
        
        private CharacterSettings _settings;
        private AttackFactory _attackFactory;
        private Coroutine _waitFlyTimeRoutine;
        private Vector3 _targetPosition;
        private Vector3 _originalPosition;
        private bool _canMakeDamage;

        public override void Release()
        {
            _trail.Stop();
            StopMoving();
            DisableDamageDealing();
            base.Release();
        }

        public void DisableDamageDealing() => _canMakeDamage = false;

        public void Launch(CharacterSettings settings, Transform target, AttackFactory attackFactory)
        {
            _settings = settings;
            _attackFactory = attackFactory;
            Vector3 position = transform.position;
            Vector3 direction = (target.position - position).normalized;
            _targetPosition = position + direction * 20f;
            _originalPosition = position;
            _trail.Play();
            
            StopMoving();
            _waitFlyTimeRoutine = StartCoroutine(ProjectileFly());
            EnableDamageDealing();
        }

        private IEnumerator ProjectileFly()
        {
            float percent = 0;
            
            while (percent < 1)
            {
                percent += Time.deltaTime / _settings.ProjectileFlyTime;
                transform.position = Vector3.Lerp(_originalPosition, _targetPosition, percent);
                yield return null;
            }
            
            Release();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_canMakeDamage)
                Collide(other);
        }

        private void EnableDamageDealing() => _canMakeDamage = true;

        private void Collide(Collider other)
        {
            StopMoving();
            PlayHitParticle(transform.position);
            TryMakeDamage(other);
            Release();
        }

        private void TryMakeDamage(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_settings.Damage);
        }
        
        private void PlayHitParticle(Vector3 point)
        {
            var hitParticle = _attackFactory.GetHitParticle(point);
            hitParticle.Play();
            hitParticle.ReleaseAfter(1f);
        }

        private void StopMoving()
        {
            if (_waitFlyTimeRoutine != null)
            {
                StopCoroutine(_waitFlyTimeRoutine);
                _waitFlyTimeRoutine = null;
            }
        }
    }
}