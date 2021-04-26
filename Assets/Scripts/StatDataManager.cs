using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public struct GameData
    {
        public int score;
        public int level;
        public int kills;
        public int items;
    }

    public class StatDataManager : MonoBehaviour
    {
        public static StatDataManager Singleton;

        public int Score { get; private set; } = 0;
        public int Level { get; private set; } = 0;
        public int TotalKills { get; private set; } = 0;
        public int Items { get; private set; } = 0;

        private void Start()
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }

        public void EnemyDeathEvent()
        {
            TotalKills++;
            Score += 100;
        }

        public void AddPoints(int gain)
        {
            Items++;
            Score += gain;
        }

        public void LevelIncrease()
        {
            Level++;
        }

        public GameData GetGameData()
        {
            GameData data = new GameData()
            {
                score = Score,
                level = Level,
                kills = TotalKills,
                items = Items
            };

            return data;
        }

        public void ItemPicked()
        {
            Items++;
        }

        public void OnPlayerDeath()
        {          
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }
    }
}