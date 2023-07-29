using UnityEngine;

namespace Gameplay
{
    public static class Statistics
    {
        private const string PLAYER_LEVEL = "PLAYER_LEVEL_INDEX";
        private const string CURRENT_LEVEL = "PLAYER_LEVEL_INDEX";
        private const string PLAYER_REWARD = "PLAYER_REWARD";

        public static int PlayerLevel
        {
            get => GetInt(PLAYER_LEVEL, 1);
            set => SetInt(PLAYER_LEVEL, value);
        }
        
        public static int CurrentLevel
        {
            get => GetInt(CURRENT_LEVEL);
            set => SetInt(CURRENT_LEVEL, value);
        }
        
        public static int PlayerReward
        {
            get => GetInt(PLAYER_REWARD);
            set => SetInt(PLAYER_REWARD, value);
        }

        private static int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

        private static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
    }
}