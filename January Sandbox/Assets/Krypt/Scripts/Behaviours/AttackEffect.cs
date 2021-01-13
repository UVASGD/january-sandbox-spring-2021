using UnityEngine;
using System.Collections;

namespace Krypt {
    public class AttackEffect : SFX_Object {

        public Enemy owner;
        public AttackType type;

        private Player player;

        private void Start() {
            player = KryptController.instance.player;
        }

        private void OnCollisionEnter(Collision collision) {
            Transform tr = collision.rigidbody.transform;
            if (tr.GetComponentInParent<Player>()) {

            }
        }

    }
}