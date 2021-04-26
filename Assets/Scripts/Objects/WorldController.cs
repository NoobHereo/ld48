using UnityEngine;
using SuperTiled2Unity;
using System.Collections;
using TMPro;

namespace game.Objects
{
    public class WorldController : MonoBehaviour
    {
        //============= INTERACTABLES =============//
        public SuperMap[] Levels;
        public GameObject Trapdoor;

        public GameObject[] EasyEnemies;
        public GameObject[] DifficultEnemies;
        public GameObject[] HardcoreEnemies;
        public GameObject[] Bossess;

        public GameObject CurrentMap;

        //============= STATS =============//
        [SerializeField] private int currentWorldId;
        [SerializeField] private int difficulty = 1;        
        [SerializeField] private int remainingEnts;
        [SerializeField] private bool currentArenaDone = false;
        public int EnemyCount = 5;

        private void Start()
        {
            StartWave();
        }

        public void LoadMap(int mapId)
        {
            if (CurrentMap != null)
                RemoveMap();

            var mapGO = GameObject.Instantiate(Levels[mapId].gameObject);
            mapGO.transform.localPosition = Vector2.zero;
            if (currentArenaDone)
                currentArenaDone = false;            
            EnemyCount += 1;
            CurrentMap = mapGO;
            currentWorldId = mapId;
            StatDataManager.Singleton.LevelIncrease();
        }

        public void StartWave()
        {
            remainingEnts = EnemyCount;
            for (int i = 0; i < EnemyCount; i++)
            {
                float x = Random.Range(6.25f, 24f);
                float y = Random.Range(-6.25f, -23f);
                GameObject randomEnt;

                if (currentWorldId <= 0)
                    randomEnt = GetRandomEnemy(1, false);
                else
                    randomEnt = GetRandomEnemy(currentWorldId + 1, false);


                StartCoroutine(SpawnEntity(randomEnt, new Vector3(x, y, 0), Quaternion.identity));      
            }
        }

        public void RemoveMap()
        {
            Debug.Log("Removing map");
            GameObject.Destroy(CurrentMap);
        }

        private GameObject GetRandomEnemy(int difficulty, bool boss)
        {
            GameObject entity = null;

            if (boss)
                entity = Bossess[Random.Range(0, Bossess.Length)];

            switch (difficulty)
            {
                case 1:
                case 2:
                case 3:
                    entity = EasyEnemies[Random.Range(0, EasyEnemies.Length)]; // Easy
                    break;

                case 4:
                case 5:
                case 6:      
                    entity = DifficultEnemies[Random.Range(0, DifficultEnemies.Length)]; // Difficult
                    break;

                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    entity = HardcoreEnemies[Random.Range(0, HardcoreEnemies.Length)];
                    break;

                default:
                    entity = Bossess[Random.Range(0, Bossess.Length)];
                    break;

            }

            return entity;
        }

        private IEnumerator SpawnEntity(GameObject entity, Vector3 position, Quaternion rotation)
        {
            float minSpawnT = 5f;
            float maxSpawnT = 10f;

            entity.GetComponent<Enemy>().WorldController = this;
            float randomTime = Random.Range(minSpawnT, maxSpawnT);
            yield return new WaitForSeconds(randomTime);
            Instantiate(entity, position, rotation);           
        }

        public void OnWorldEntityDeath()
        {
            remainingEnts--;
            StatDataManager.Singleton.EnemyDeathEvent();
        }

        private void Update()
        {
            if (remainingEnts == 0 && !currentArenaDone)
            {
                currentArenaDone = true;
                difficulty++;
                if (difficulty == 19)
                {
                    Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                    player.HasWon = true;
                }
                    
                UnlockNextLevel();
            }
        }

        private void UnlockNextLevel()
        {
            Trapdoor.GetComponent<TrapDoor>().Unlock();
        }     
    }
}