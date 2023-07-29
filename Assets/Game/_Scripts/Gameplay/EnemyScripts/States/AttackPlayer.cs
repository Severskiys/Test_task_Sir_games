using _ScriptableObjects.Classes;
using Gameplay.DamageProcessing;
using Gameplay.PlayerScripts;
using UnityEngine;
using Utils;
using Utils.StateMachine;

namespace Gameplay.EnemyScripts.States
{
    public class AttackPlayer : IState
    {
        private readonly EnemyCharacter _enemyBase;
        private readonly Weapon _weapon;
        private readonly Player _player;
        private readonly CharacterView _enemyView;
        private Transform _selfTransform;
        private EnemySettings _enemySettings;

        public AttackPlayer(EnemyCharacter enemyBase, Weapon weapon, CharacterView characterView, EnemySettings enemySettings, Player player)
        {
            _enemySettings = enemySettings;
            _enemyBase = enemyBase;
            _weapon = weapon;
            _player = player;
            _enemyView = characterView;
        }

        public void Tick()
        {
            if (_player.IsAlive)
                RotateTowardCharacter();

            _enemyBase.Timer -= Time.deltaTime;
        }

        public void OnEnter()
        {
            _enemyBase.Timer = _enemySettings.AttackTimer;
            _selfTransform = _enemyBase.transform;
            _enemyView.HandsUp();
            _enemyView.PlayIdle();
            if (_player.IsAlive)
                _weapon.StartAttack(_player.TargetToAim);
        }

        public void OnExit()
        {
            _enemyView.HandsDown();
            _weapon.StopAttack();
        }
        
        private void RotateTowardCharacter() => _selfTransform.HorizontalSoftLookAt(_player.transform, 15f);
        
    }
}