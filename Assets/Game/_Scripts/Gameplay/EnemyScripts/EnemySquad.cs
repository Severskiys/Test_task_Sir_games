using System;
using System.Collections.Generic;
using Gameplay.PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.EnemyScripts
{
    [SelectionBase]
    public class EnemySquad : MonoBehaviour
    {
        public event Action OnAllEnemiesEliminated;
        public event Action<int> OnEnemyEliminated;

        [SerializeField] private List<EnemyCharacter> _enemiesPrefabs;
        [SerializeField] private EnemySpawnPoint _spawnPoint;
        [SerializeField] private int _spawnCount;
        [SerializeField] private Player _player;
        
        private List<EnemyCharacter> _spawnedEnemies = new List<EnemyCharacter>();
        public bool StartAttackPlayer { get; private set; }

        public void ActivateEnemies() => StartAttackPlayer = true;

        private void Awake() => InitSquad();

        private void OnDisable()
        {
            foreach (var enemy in _spawnedEnemies)
                enemy.OnDeath -= RemoveCharacterFromUnit;
        }

        private void InitSquad()
        {
            _spawnedEnemies.Clear();

            for (int i = 0; i < _spawnCount; i++)
            {
                var enemy = Instantiate(_enemiesPrefabs[Random.Range(0, _enemiesPrefabs.Count)], 
                    _spawnPoint.GetPoint(), Quaternion.identity, transform);
                _spawnedEnemies.Add(enemy);
                enemy.Init(_player, this);
                enemy.OnDeath += RemoveCharacterFromUnit;
            }
        }

        private void RemoveCharacterFromUnit(EnemyCharacter enemyCharacter)
        {
            enemyCharacter.OnDeath -= RemoveCharacterFromUnit;
            OnEnemyEliminated?.Invoke(enemyCharacter.CoinsForKill);
            _spawnedEnemies.Remove(enemyCharacter);
            if (_spawnedEnemies.Count == 0)
                OnAllEnemiesEliminated?.Invoke();
        }
    }
}