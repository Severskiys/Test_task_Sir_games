using _ScriptableObjects.Classes;
using DG.Tweening;
using Gameplay.DamageProcessing;
using Gameplay.EnemyScripts;
using UnityEngine;
using Utils;

namespace Gameplay.PlayerScripts
{
    public class AutoAim : MonoBehaviour
    {
        [SerializeField] private Transform _bodyToRotate;
        [SerializeField] private Transform _bulletSpawnPoint;
        
        private Transform _currentTarget;
        private Coroutine _lookAtTargetRoutine;
        private Tween _rotateToIdentity;
        private Weapon _weapon;

        public void Init(CharacterSettings settings, CharacterView characterView) 
            => _weapon = new Weapon(settings, _bulletSpawnPoint, characterView);

        public void LookAtTarget() 
            => _bodyToRotate.HorizontalSoftLookAt(_currentTarget, 14);

        public void StartShoot(Transform target)
        {
            _currentTarget = target;
            _rotateToIdentity?.Kill();
            _weapon.StartAttack(_currentTarget);
        }

        public void StopShoot()
        {
            _weapon.StopAttack();
            _rotateToIdentity?.Kill();
            _rotateToIdentity = _bodyToRotate.DOLocalRotateQuaternion(Quaternion.identity, 0.15f);
        }

        public void SwitchTarget(Transform target)
        {
            _currentTarget = target;
            _weapon.SwitchTarget(_currentTarget);
        }
    }
}