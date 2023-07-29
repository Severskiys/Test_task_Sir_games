using System;
using _ScriptableObjects.Classes;
using Gameplay.DamageProcessing;
using Gameplay.EnemyScripts.States;
using Gameplay.PlayerScripts;
using UnityEngine;
using UnityEngine.AI;
using Utils.StateMachine;

namespace Gameplay.EnemyScripts
{
    public abstract class EnemyCharacter : MonoBehaviour
    {
        public event Action<EnemyCharacter> OnDeath;
        
        [SerializeField] private EnemySettings _settings;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CharacterView _enemyView;
        [SerializeField] private DamageTaker _damageTaker;
        [SerializeField] private Transform _spawnProjectilePoint;
        
        private Weapon _weapon;
        private StateMachine _stateMachine;
        private Player _player;
        private EnemySquad _enemySquad;
        private bool _stateMachineInit;
       
        public float Timer { get; set; }
        public bool IsAlive => _damageTaker.IsAlive;
        public int CoinsForKill => _settings.CoinsForKill;
        public Transform TargetToAim => _damageTaker.TargetToAim;

        public void DoOnDeath()
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject, 6.0f);
        }

        public void Init(Player player, EnemySquad enemySquad)
        {
            _enemySquad = enemySquad;
            _player = player;
            _damageTaker.Init(_settings);
            _agent.speed = _settings.Speed;
            CreateWeapon();
            SetStateMachine();
        }

        private void CreateWeapon() => _weapon = new Weapon(_settings, _spawnProjectilePoint, _enemyView);

        private void SetStateMachine()
        {
            _stateMachine = new StateMachine();
            var stayIdle = new StayIdle(this, _enemyView);
            var runAtPlayerState = new RunAtPlayer(this, _agent, _enemyView, _settings, _player);
            var attackPlayerCharacter = new AttackPlayer(this, _weapon, _enemyView, _settings, _player);
            var enemyDie = new EnemyDie(this, _enemyView);

            _stateMachine.AddTransition(stayIdle, runAtPlayerState, () => _enemySquad.StartAttackPlayer);
            _stateMachine.AddTransition(attackPlayerCharacter, runAtPlayerState, () => Timer <= 0);
            _stateMachine.AddTransition(runAtPlayerState, attackPlayerCharacter, () => runAtPlayerState.CompleteRun);
            _stateMachine.AddAnyTransition(enemyDie, () => IsAlive == false);
            _stateMachine.AddAnyTransition(stayIdle, () => _player.IsAlive == false);
            
            _stateMachine.SetState(stayIdle);
            _stateMachineInit = true;
        }

        private void Update()
        {
            if (_stateMachineInit) 
                _stateMachine.Tick();
        }
    }
}