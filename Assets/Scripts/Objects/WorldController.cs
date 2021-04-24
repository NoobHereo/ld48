using UnityEngine;
using SuperTiled2Unity;
using System.Collections;

namespace game.Objects
{
    public class WorldController : MonoBehaviour
    {
        public SuperMap[] Levels;
        public GameObject[] Trapdoors;

        public GameObject[] EasyEnemies;
        public GameObject[] DifficultEnemies;
        public GameObject[] HardcoreEnemies;
        public GameObject[] Bossess;

        public GameObject CurrentMap;
        private int currentWorldId;
        private int difficulty = 1;
        public int EnemyCount = 10;
        private int remainingEnts;
        private bool currentArenaDone = false;

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
            difficulty++;
            EnemyCount += 5;
            CurrentMap = mapGO;
            currentWorldId = mapId;
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
                    randomEnt = GetRandomEnemy(currentWorldId, false);


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
                    entity = EasyEnemies[Random.Range(0, EasyEnemies.Length)];
                    break;

                case 2:
                    entity = DifficultEnemies[Random.Range(0, DifficultEnemies.Length)];
                    break;

                case 3:
                    entity = HardcoreEnemies[Random.Range(0, HardcoreEnemies.Length)];
                    break;
            }

            return entity;
        }

        private IEnumerator SpawnEntity(GameObject entity, Vector3 position, Quaternion rotation)
        {
            float minSpawnT = 1f / difficulty;
            float maxSpawnT = 10f / difficulty;

            entity.GetComponent<Enemy>().WorldController = this;
            float randomTime = Random.Range(minSpawnT, maxSpawnT);
            yield return new WaitForSeconds(randomTime);
            Instantiate(entity, position, rotation);           
        }

        public void OnWorldEntityDeath()
        {
            remainingEnts--;
            Debug.Log("One enemy down, " + remainingEnts + " to go!");
        }

        private void Update()
        {
            if (remainingEnts == 0 && !currentArenaDone)
            {
                currentArenaDone = true;
                UnlockNextLevel();
            }
        }

        private void UnlockNextLevel()
        {
            Trapdoors[currentWorldId].GetComponent<TrapDoor>().Unlock();
        }
    }
}