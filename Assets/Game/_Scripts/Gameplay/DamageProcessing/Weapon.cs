using System;
using System.Collections;
using _ScriptableObjects.Classes;
using Gameplay.PlayerScripts;
using UnityEngine;
using Utils;

namespace Gameplay.DamageProcessing
{
    public class Weapon
    {
        public event Action OnMakeHit;

        private CharacterSettings _settings;
        private Transform _currentDamageable;
        private Coroutine _shootRoutine;
        private Transform _spawnPoint;
        private CharacterView _characterView;
        private Projectile _projectile;
        private bool _waitAttack;
        private bool _hasProjectile;
        private AttackFactory _attackFactory;

        public Weapon(CharacterSettings settings, Transform spawnPoint, CharacterView characterView)
        {
            _settings = settings;
            _characterView = characterView;
            _spawnPoint = spawnPoint;
            _attackFactory = new AttackFactory(settings);
        }
        
        public void StartAttack(Transform target)
        {
            _currentDamageable = target;
            if (_shootRoutine == null)
                _shootRoutine = Level.Parent.StartCoroutine(ShootRoutine());
        }

        public void SwitchTarget(Transform target) => _currentDamageable = target;

        public void StopAttack()
        {
            if (_shootRoutine != null)
            {
                Level.Parent.StopCoroutine(_shootRoutine);
                _shootRoutine = null;
            }
        }

        private IEnumerator ShootRoutine()
        {
            while (true)
            {
                yield return MakeAttack();
                OnMakeHit?.Invoke();
                yield return Helper.GetWait(_settings.DelayAfterAttack);
            }
        }

        private IEnumerator MakeAttack()
        {
            SpawnProjectile();
            _characterView.OnMakeAttack += LaunchProjectile;
            _characterView.PlayAttack();
            _waitAttack = true;
            
            while (_waitAttack)
                yield return null;
        }
        
        private void SpawnProjectile()
        {
            if (_hasProjectile == false)
            {
                _projectile = _attackFactory.GetProjectile(_spawnPoint);
                _projectile.DisableDamageDealing();
                _hasProjectile = true;
            }
        }

        private void LaunchProjectile()
        {
            _characterView.OnMakeAttack -= LaunchProjectile;
            _projectile.SetParent(Level.Parent.transform);
            _projectile.Launch(_settings, _currentDamageable, _attackFactory);
            _hasProjectile = false;
            _waitAttack = false;
        }
    }
}