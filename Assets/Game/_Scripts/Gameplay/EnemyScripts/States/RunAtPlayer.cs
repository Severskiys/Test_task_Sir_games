using System.Collections.Generic;
using _ScriptableObjects.Classes;
using Gameplay.PlayerScripts;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Utils.StateMachine;

namespace Gameplay.EnemyScripts.States
{
    public class RunAtPlayer : IState
    {
        private readonly EnemyCharacter _enemyBase;
        private readonly NavMeshAgent _agent;
        private readonly CharacterView _enemyView;
        private readonly EnemySettings _settings;
        private readonly Player _player;

        private Vector3 _targetToRunAt;
        private float _safeTimer;
        private float _safeTimerValue = 5.0f;
     
        public RunAtPlayer(EnemyCharacter enemyBase, NavMeshAgent agent, CharacterView enemyView, EnemySettings settings, Player player)
        {
            _enemyBase = enemyBase;
            _agent = agent;
            _enemyView = enemyView;
            _settings = settings;
            _player = player;
        }

        public bool CompleteRun { get; private set; }

        public void Tick()
        {
            _enemyView.AnimateWalk(_agent.velocity.magnitude / _settings.Speed);
            
            if (Vector3.Distance(_enemyBase.transform.position, _targetToRunAt) < 1.5f)
                CompleteRun = true;

            _safeTimer -= Time.deltaTime;

            if (_safeTimer < 0)
                CompleteRun = true;
        }

        public void OnEnter()
        {
            CompleteRun = false;
            GenerateTargetPosition();
            _agent.enabled = true;
            _agent.isStopped = false;
            _agent.SetDestination(_targetToRunAt);
            
            _safeTimer = _safeTimerValue;
        }

        private void GenerateTargetPosition()
        {
            if (_settings.NeedToSeePlayer == false)
                GenerateRandomPoint();
            else
                GeneratePointToSeePlayer();
            
        }

        private void GeneratePointToSeePlayer()
        {
            var pointsAroundPlayer = GeneratePointsAroundPlayer(out var playerPosition);
            var seePlayerPoints = CheckSeePlayerPoints(pointsAroundPlayer, playerPosition);
            
            if (seePlayerPoints.Count > 0)
                _targetToRunAt = seePlayerPoints[Random.Range(0, seePlayerPoints.Count)].GenerateRandomPosition(1, 1);
            else
                GenerateRandomPoint();
        }

        private List<Vector3> GeneratePointsAroundPlayer(out Vector3 playerPosition)
        {
            List<Vector3> pointsAroundPlayer = new List<Vector3>();

            playerPosition = _player.transform.position;

            pointsAroundPlayer.Add(playerPosition + new Vector3(_settings.TargetBordersAroundPlayer.x * 0.5f, 0,
                _settings.TargetBordersAroundPlayer.y * 0.5f));
            pointsAroundPlayer.Add(playerPosition + new Vector3(-_settings.TargetBordersAroundPlayer.x * 0.5f, 0,
                -_settings.TargetBordersAroundPlayer.y * 0.5f));
            pointsAroundPlayer.Add(playerPosition + new Vector3(-_settings.TargetBordersAroundPlayer.x * 0.5f, 0,
                _settings.TargetBordersAroundPlayer.y * 0.5f));
            pointsAroundPlayer.Add(playerPosition + new Vector3(_settings.TargetBordersAroundPlayer.x * 0.5f, 0,
                -_settings.TargetBordersAroundPlayer.y * 0.5f));
            pointsAroundPlayer.Add(playerPosition + new Vector3(_settings.TargetBordersAroundPlayer.x * 0.5f, 0, 0));
            pointsAroundPlayer.Add(playerPosition + new Vector3(-_settings.TargetBordersAroundPlayer.x * 0.5f, 0, 0));
            pointsAroundPlayer.Add(playerPosition + new Vector3(0, 0, _settings.TargetBordersAroundPlayer.y * 0.5f));
            pointsAroundPlayer.Add(playerPosition + new Vector3(0, 0, -_settings.TargetBordersAroundPlayer.y * 0.5f));
            return pointsAroundPlayer;
        }

        private List<Vector3> CheckSeePlayerPoints(List<Vector3> pointsAroundPlayer, Vector3 playerPosition)
        {
            List<Vector3> seePlayerPoints = new List<Vector3>();
            foreach (var point in pointsAroundPlayer)
            {
                Vector3 direction = (playerPosition - point).normalized;
                if (Physics.Raycast(point, direction, out RaycastHit hitInfo, 30f, _settings.LayerMask)
                    && hitInfo.transform.TryGetComponent(out Player player))
                {
                    seePlayerPoints.Add(point);
                }
            }

            return seePlayerPoints;
        }

        private void GenerateRandomPoint()
        {
            _targetToRunAt = _player.transform.position.GenerateRandomPosition(_settings.TargetBordersAroundPlayer.x,
                _settings.TargetBordersAroundPlayer.y);
        }

        public void OnExit()
        {
            _agent.isStopped = true;
        }
    }
}