using UnityEngine;
using System.Collections;

namespace Krypt {
    public class KryptController : MonoBehaviour {
        public static KryptController instance;

        [Header("Player Targets")]
        public Transform leftPlatform;
        public Transform middlePlatform;
        public Transform rightPlatform;

        [HideInInspector] public Player player;

        private void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }
            instance = this;
        }

        void Start() {
            var go = GameObject.FindGameObjectWithTag("Player");
            player = go.GetComponent<Player>();
        }

        void Update() {

        }
    }
}