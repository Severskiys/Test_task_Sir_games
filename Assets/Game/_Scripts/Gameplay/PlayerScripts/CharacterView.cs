using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.PlayerScripts
{
    public class CharacterView : MonoBehaviour
    {
        public event Action OnMakeAttack;
        
        [SerializeField] private Animator _animator;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int DoDie = Animator.StringToHash("DoDie");
        private static readonly int DoAttack = Animator.StringToHash("DoAttack");

        private Coroutine _goIdleRoutine;
        private float _deathZShift;

        public void AttackEvent() => OnMakeAttack?.Invoke();

        public void AnimateWalk(float speed) => _animator.SetFloat(Speed, speed);

        public void PlayDeath()
        {
            _deathZShift = 3.0f;
            transform.DOMoveY(transform.position.y - _deathZShift, 3.0f).SetDelay(2.0f).SetLink(gameObject);
            _animator.SetBool(DoDie, true);
        }

        public void PlayIdle()
        {
            StopIdleRoutine();
            _goIdleRoutine = StartCoroutine(GoIdleRoutine(.15f));
        }

        private void StopIdleRoutine()
        {
            if (_goIdleRoutine != null)
            {
                StopCoroutine(_goIdleRoutine);
                _goIdleRoutine = null;
            }
        }

        public void PlayAttack() => _animator.SetTrigger(DoAttack);

        public void HandsUp() => _animator.SetLayerWeight(1,1.0f);

        public void HandsDown() => _animator.SetLayerWeight(1,0.0f);

        private IEnumerator GoIdleRoutine(float timer)
        {
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime / timer;
                _animator.SetFloat(Speed, 0.25f - 0.25f * percent);
                yield return null;
            }
        }
    }
}