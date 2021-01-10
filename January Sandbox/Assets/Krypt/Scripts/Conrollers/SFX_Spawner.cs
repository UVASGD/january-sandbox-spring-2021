using UnityEngine;
using System.Collections;

namespace Krypt {
    public class SFX_Spawner : MonoBehaviour {

        public static SFX_Spawner instance;
        private GameObject holder;

        private void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
            holder = new GameObject("FX Objects");
        }

        public GameObject SpawnFX(GameObject fx, Vector3 position, float vol = -1, Transform parent = null) {
            return SpawnFX(fx, position, Vector3.forward, vol, parent);
        }

        public GameObject SpawnFX(GameObject fx, Vector3 position, Vector3 rotation, float vol = -1, Transform parent = null) {
            GameObject spawned_fx = Instantiate(fx, position, Quaternion.identity);
            spawned_fx.transform.parent = parent ? parent : holder.transform;

            if (rotation != Vector3.zero)
                spawned_fx.transform.forward = rotation;
            SFX_Object fx_obj = spawned_fx.GetComponent<SFX_Object>();
            fx_obj.vol = vol;

            return spawned_fx;
        }
    }
}