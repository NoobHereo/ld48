using UnityEngine;
using SuperTiled2Unity;

namespace game.Objects
{
    public class WorldController : MonoBehaviour
    {
        public SuperMap[] Levels;
        public GameObject EnemyPrefab;
        public GameObject CurrentMap;
        private int currentWorldId;

        public int EnemyCount = 10;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartWave();
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
                float x = Random.Range(6.25f, 24f);
                float y = Random.Range(-6.25f, -23f);              
                Instantiate(EnemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        public void RemoveMap()
        {
            Debug.Log("Removing map");
            GameObject.Destroy(CurrentMap);
        }
    }
}