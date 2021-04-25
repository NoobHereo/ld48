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
        public float time;
    }

    public class StatDataManager : MonoBehaviour
    {
        public static StatDataManager Singleton;

        public int Score { get; private set; } = 0;
        public int Level { get; private set; } = 0;
        public int TotalKills { get; private set; } = 0;
        public int Items { get; private set; } = 0;
        // public float TimeSpent { get; private set; } = 0;

        private bool timeRunning = false;
        private float timer = 0;

        private void Start()
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }

        private void FixedUpdate()
        {
            if (timeRunning)
            {               
                timer += Time.fixedDeltaTime;
            }
           
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

        public void StartTime()
        {
            timeRunning = true;
        }

        public GameData GetGameData()
        {
            GameData data = new GameData()
            {
                score = Score,
                level = Level,
                kills = TotalKills,
                items = Items,
                time = timer
            };

            return data;
        }

        public void ItemPicked()
        {
            Items++;
        }

        public void OnPlayerDeath()
        {
            timeRunning = false;            
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }
    }
}