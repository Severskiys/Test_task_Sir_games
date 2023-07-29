using _ScriptableObjects.Classes;
using Gameplay.EnemyScripts;
using UnityEngine;
using Utils.StateMachine;

namespace Gameplay.PlayerScripts.States
{
    public class AttackAfterStopState : IState
    {
        private readonly Player _player;
        private readonly AutoAim _autoAim;
        private readonly CharacterSettings _settings;
        private readonly CharacterView _characterView;
        private EnemyCharacter _currentTarget;

        public AttackAfterStopState(Player player, AutoAim autoAim, CharacterView characterView)
        {
            _player = player;
            _autoAim = autoAim;
            _characterView = characterView;
        }

        public void Tick()
        {
            if (_player.CanAttackTarget)
            {
                _autoAim.LookAtTarget();
                TrySwitchTarget();
            }
        }

        private void TrySwitchTarget()
        {
            if (_currentTarget.IsAlive == false)
                _autoAim.SwitchTarget(GetTarget());
        }

        public void OnEnter()
        {
            _characterView.HandsUp();
            _characterView.PlayIdle();
            if (_player.CanAttackTarget)
                _autoAim.StartShoot(GetTarget());
        }

        public void OnExit()
        {
            _autoAim.StopShoot();
        }

        private Transform GetTarget()
        {
            _currentTarget = _player.GetClosestTarget();
            return _currentTarget.TargetToAim;
        }
    }
}