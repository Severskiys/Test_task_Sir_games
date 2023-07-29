using System;
using _ScriptableObjects.Classes;
using Gameplay.DamageProcessing;
using Gameplay.EnemyScripts;
using Gameplay.PlayerScripts.States;
using Gameplay.Zones;
using UnityEngine;
using Utils.StateMachine;

namespace Gameplay.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        public event Action OnDeath;

        [SerializeField] private CharacterView _characterView;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private TargetLocator _targetLocator;
        [SerializeField] private DamageTaker _damageTaker;
        [SerializeField] private AutoAim _autoAim;
        [SerializeField] private CharacterSettings _settings;

        private StateMachine _stateMachine;
        private bool _stateMachineInit;
        public bool IsAlive => _damageTaker.IsAlive;
        public bool CanAttackTarget => _playerMovement.IsStopped && _targetLocator.HasTargetInRange;
        public Transform TargetToAim => _damageTaker.TargetToAim;

        public EnemyCharacter GetClosestTarget() => _targetLocator.GetClosestTarget(transform.position);

        public void ActivatePlayer()
        {
            _autoAim.Init(_settings, _characterView);
            _damageTaker.Init(_settings);
            _playerMovement.Init(_settings, _characterView);
            _targetLocator.Init(_settings);
            
            SetStateMachine();
        }

        public void ProcessDeath()
        {
            OnDeath?.Invoke();
            _characterView.PlayDeath();
        }

        private void SetStateMachine()
        {
            _stateMachine = new StateMachine();
            
            var moveState = new MoveState(this, _characterView);
            var attackAfterStop = new AttackAfterStopState(this, _autoAim, _characterView);
            var die = new DeathState(this);
            
            _stateMachine.AddTransition(moveState, attackAfterStop, () => CanAttackTarget);
            _stateMachine.AddTransition(attackAfterStop, moveState, () => CanAttackTarget == false);
            _stateMachine.AddAnyTransition(die, () => IsAlive == false);
            _stateMachine.SetState(moveState);
            _stateMachineInit = true;
        }

        private void Update()
        {
            if (_stateMachineInit)
                _stateMachine.Tick();
        }
    }
}