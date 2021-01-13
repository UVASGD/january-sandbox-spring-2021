using UnityEngine;
using System.Collections;

namespace Krypt {
    [RequireComponent(typeof(Animator))]
    public class AnimPulsar : MonoBehaviour {

        // typically the animation with have a crest and a trough, so rate should be halved
        public Quantize beatRate = Quantize.Half;
        private int sampleRate = 60;
        Animator anim;

        private void Start() {
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate() {
            float speed = 0;
            if (SongController.instance.IsPlaying) {
                // animation clip should be a length of 1 sec
                var song = SongController.instance.currentSong;
                speed = (song.BPM * SongController.instance.playSpeed) / 
                    (Mathf.Pow(2, (int)beatRate) * sampleRate) ;
            }

            anim.SetFloat("Speed", speed);
        }

        // Reset the current animation state to match time
        private void ResetAnim() {
            var state = anim.GetCurrentAnimatorStateInfo(0);
            anim.Play(state.fullPathHash, 0, 0);
        }
    }
}