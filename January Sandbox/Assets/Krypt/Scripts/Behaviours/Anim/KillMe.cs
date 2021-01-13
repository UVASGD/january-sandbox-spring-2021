using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Krypt {
    public class KillMe : StateMachineBehaviour {
        // OnStateExit is called before OnStateExit is called on any state inside this state machine
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

            GameObject go = animator.gameObject;
            var en = animator.transform.parent.GetComponent<Enemy>();
            if (en != null) go = en.gameObject;

            Destroy(go);
        }

    }
}