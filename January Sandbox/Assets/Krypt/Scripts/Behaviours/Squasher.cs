using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Krypt {

    public class Squasher : MonoBehaviour {

        [Header("Pulse Timing")]
        public bool offbeat = false;
        public float speed = 2.5f;
        public float duration = 0.5f;
        public float killSpeed = 0.5f;

        [Header("Pulse Scaling (Local Space)")]
        public float majorScaleAmplitude = 1.25f;
        public float minorScaleAmplitude = 1.17f;
        public AnimationCurve scaleFalloff;

        protected float fallTime;
        protected Vector3 initialScale;
        protected float scaleAmplitude;

        public float intensity { get; protected set; }

        protected virtual void Start() {
            if (!offbeat) {
                SongController.instance.MajorNote += MajorNote;
                SongController.instance.MinorNote += MinorNote;
            } else {
                SongController.instance.OffbeatNote += MinorNote;
            }

            initialScale = transform.localScale;
        }

        void Update() {

            if (SongController.instance.IsPlaying) {
                fallTime += Time.deltaTime;
                Vector3 s = Vector3.Lerp(initialScale, initialScale * scaleAmplitude,
                    scaleFalloff.Evaluate(Mathf.Clamp01(fallTime / duration)));
                transform.localScale = s;
            } else {
                Vector3 s = Vector3.Lerp(initialScale, transform.localScale,
                    Time.deltaTime * killSpeed);
                transform.localScale = s;
            }
        }

        public virtual void MajorNote() {
            scaleAmplitude = majorScaleAmplitude;
            NoteHit();
        }

        public virtual void MinorNote() {
            scaleAmplitude = minorScaleAmplitude;
            NoteHit();
        }

        private void NoteHit() {
            fallTime = 0;
        }
    }
}