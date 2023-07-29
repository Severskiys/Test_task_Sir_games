using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private List<Level> _levels;
        [SerializeField] private Transform _levelsParent;

        private Level _currentLevel;

        private void Start()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            UnloadLevel();

            if (Statistics.CurrentLevel >= _levels.Count)
                Statistics.CurrentLevel = _levels.Count - 1;
            
            _currentLevel = Instantiate(_levels[Statistics.CurrentLevel], _levelsParent);
            _currentLevel.transform.localPosition = Vector3.zero;
            
            _currentLevel.OnWinLevel += ProcessWinLevel;
            _currentLevel.OnLooseLevel += ProcessLooseLevel;
        }

        private void UnloadLevel()
        {
            if (_currentLevel == null) 
                return;
            
            Destroy(_currentLevel.gameObject);
        }

        private void IncreaseLevelNumber()
        {
            Statistics.CurrentLevel++;
            if (Statistics.CurrentLevel >= _levels.Count)
                Statistics.CurrentLevel = 0;
            
            Statistics.PlayerLevel++;
        }
        
        private void ProcessLooseLevel()
        {
            _currentLevel.OnWinLevel -= ProcessWinLevel;
            _currentLevel.OnLooseLevel -= ProcessLooseLevel;
            LoadLevel();
        }

        private void ProcessWinLevel()
        {
            _currentLevel.OnWinLevel -= ProcessWinLevel;
            _currentLevel.OnLooseLevel -= ProcessLooseLevel;
            IncreaseLevelNumber();
            LoadLevel();
        }
    }
}