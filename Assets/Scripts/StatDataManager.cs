using UnityEngine;

namespace game
{
    public class StatDataManager : MonoBehaviour
    {
        public static StatDataManager Singleton;
        public int EnemiesKilled { get; private set; }

        private void Start()
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }

        public void EnemyDeathEvent()
        {
            EnemiesKilled++;
        }

    }
}