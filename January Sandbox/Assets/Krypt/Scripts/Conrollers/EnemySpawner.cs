using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Krypt {
    public class EnemySpawner : MonoBehaviour {
        public static EnemySpawner instance;

        //private List<Enemy> enemyList = new List<Enemy>();
        [Header("Spawn Platforms")]
        public List<Transform> front;
        public List<Transform> middle;
        public List<Transform> back;
        public List<List<Transform>> platformMap { private set; get; }

        [Header("Customization")]
        public List<GameObject> spawnFXList;
        public List<GameObject> enemyPrefabList;
            
        private Dictionary<EnemyType, GameObject> spawnEffects = new Dictionary<EnemyType, GameObject>();
        private Dictionary<EnemyType, GameObject> enemyPrefabs = new Dictionary<EnemyType, GameObject>();
        private Transform spawnParent;

        void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
            var obj = GameObject.Find("Spawned Objects");
            if (obj != null) spawnParent = obj.transform;

            List<Transform>[] arr = { front, middle, back };
            platformMap = arr.ToList();

            // Unity doesn't show Dictionaries in the editor, so we have to populate it ourselves
            for(int i = 0; i < spawnFXList.Count; i++) {
                spawnEffects[(EnemyType)i] = spawnFXList[i];
            } for(int i = 0; i< enemyPrefabList.Count; i++) {
                enemyPrefabs[(EnemyType)i] = enemyPrefabList[i];
            }
            
        }

        void Update() {

        }

        public Transform GetPlatform(Vector2 i) {
            return GetPlatform((int)i.x, (int)i.y);
        }

        public Transform GetPlatform(int x, int y) {
            if (y >= platformMap.Count || y < 0
                || x >= platformMap[y].Count || x < 0)
                return null;
            return platformMap[y][x];
        }

        public void SpawnEnemy(GameObject enemyObj, Vector3 loc) {
            var e = enemyObj.GetComponent<Enemy>();
            if (e == null) return;
            SpawnEnemy(e.classification, loc);
        }

        public void SpawnEnemy(EnemyType type, Vector3 loc) { 
            var prefab = enemyPrefabs[type];
            if (prefab != null) {
                GameObject e = Instantiate(prefab, loc, Quaternion.identity);
                e.transform.parent = spawnParent ? spawnParent : transform;

                var sFX = spawnEffects[type];
                if (sFX != null) {
                    SFX_Spawner.instance.SpawnFX(sFX, loc, parent: e.transform);
                }

            } else {
                Debug.LogWarning("Prefab could not be found for EnemyType:" + type);
            }
        }
    }
}