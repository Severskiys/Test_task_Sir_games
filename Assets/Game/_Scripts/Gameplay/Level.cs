using System;
using System.Collections;
using DG.Tweening;
using Gameplay.EnemyScripts;
using Gameplay.PlayerScripts;
using Gameplay.Zones;
using TMPro;
using UnityEngine;
using Utils;
using Utils.InputClasses;

namespace Gameplay
{
    public class Level : MonoBehaviour
    {
        public event Action OnWinLevel;
        public event Action OnLooseLevel;
        
        [SerializeField] private ActiveZone _levelEndTrigger;
        [SerializeField] private Player _player;
        [SerializeField] private EnemySquad _enemySquad;
        [SerializeField] private TMP_Text _playerTotalCoins;
        [SerializeField] private TMP_Text _startLevelCounter;
        [SerializeField] private float _levelStartTimer = 3.0f;

        private int _totalReward;
        
        private static Level _instance;
        public static Level Parent => _instance;
        
        private void Awake()
        {
            _instance = this;
            StartCoroutine(LevelStartRoutine());
            _levelEndTrigger.DisableZone();
            UpdateReward(Statistics.PlayerReward);
        }
        
        private void OnEnable()
        {
            _enemySquad.OnAllEnemiesEliminated += ActivateLevelExit;
            _enemySquad.OnEnemyEliminated += UpdateReward;
            _levelEndTrigger.OnPlayerEnter += WinLevel;
            _player.OnDeath += LooseLevel;
        }

        private void OnDisable()
        {
            _enemySquad.OnAllEnemiesEliminated -= ActivateLevelExit;
            _enemySquad.OnEnemyEliminated -= UpdateReward;
            _levelEndTrigger.OnPlayerEnter -= WinLevel;
            _player.OnDeath -= LooseLevel;
        }

        private void UpdateReward(int reward)
        {
            _totalReward += reward;
            _playerTotalCoins.text = "Coins Count: " + _totalReward;
        }

        private IEnumerator LevelStartRoutine()
        {
            float timer = _levelStartTimer;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                TimeSpan span = TimeSpan.FromSeconds(timer);
                _startLevelCounter.text = $"{span.Seconds} : {span.Milliseconds}";
                yield return null;
            }

            _startLevelCounter.DOFade(0, 0.15f);
            TouchInput.Instance.TurnOnInput();
            _enemySquad.ActivateEnemies();
            _player.ActivatePlayer();
        }

        private void ActivateLevelExit() => _levelEndTrigger.EnableZone();
        
        private void WinLevel()
        {
            TouchInput.Instance.TurnOffInput();
            OnWinLevel?.Invoke();
        }

        private void LooseLevel()
        {
            TouchInput.Instance.TurnOffInput();
            OnLooseLevel?.Invoke();
        }
    }
}