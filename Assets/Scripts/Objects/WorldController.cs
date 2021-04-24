using UnityEngine;
using SuperTiled2Unity;

namespace game.Objects
{
    public class WorldController : MonoBehaviour
    {
        public Transform[] SpawnLocations;
        public SuperMap[] Levels;
        public GameObject EnemyPrefab;
        public GameObject CurrentMap;
        private int currentWorldId;

        public int EnemyCount = 10;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                LoadMap(currentWorldId + 1);
        }

        public void LoadMap(int mapId)
        {
            if (CurrentMap != null)
                RemoveMap();

            var mapGO = GameObject.Instantiate(Levels[mapId].gameObject);
            mapGO.transform.localPosition = Vector2.zero;     
            CurrentMap = mapGO;
            currentWorldId = mapId;
        }

        public void StartWave()
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                int loc = Random.Range(0, SpawnLocations.Length);
                Instantiate(EnemyPrefab, SpawnLocations[loc]);
            }
        }

        public void RemoveMap()
        {
            Debug.Log("Removing map");
            GameObject.Destroy(CurrentMap);
        }
    }
}