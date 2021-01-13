
using UnityEngine;

namespace Krypt {
    /// <summary>
    /// Allows the designer to click on an object in the scene and highlight it;
    /// Should not be active ingame
    /// </summary>
    public class RecordableObject : MonoBehaviour {

        private Object self;
        private bool IsEnemy => self.GetType() == typeof(Enemy);

        private void Awake() {
            var t = gameObject.GetComponent<Enemy>();
            if (t != null) self = t;
            else self = transform;
        }

        private void OnMouseDown() {
            if (!ActionRecorder.instance.recording) return;
            if (IsEnemy) {
                ActionRecorder.instance.OnClickEnemy((Enemy)self);
            } else ActionRecorder.instance.OnClickPlatform(transform);
        }

        private void OnMouseOver() {
            if (!ActionRecorder.instance.recording) return;
            if (!IsEnemy)
                ActionRecorder.instance.OnMouseEnterPlatform(transform);
        }

        private void OnMouseExit() {
            if (!ActionRecorder.instance.recording) return;
            if (!IsEnemy)
                ActionRecorder.instance.OnMouseExitPlatform();
        }
    }
}
