using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Krypt {

    [RequireComponent(typeof(MeshRenderer))]
    public class Pulsar : Squasher {

        Material mat;
        private List<Light> lights;

        public float intensityMul = 1;


        [Header("Pulsing Colors")]
        [ColorUsage(true, true)]
        public Color majorEmissionColor = Color.red;
        //public float majorEmissionIntensity = 6;
        [ColorUsage(true, true)]
        public Color minorEmissionColor = Color.blue;
        //public float minorEmissionIntensity = 6;
        [ColorUsage(true, true)]
        public Color silentEmissionColor = Color.black;
        //public float silentEmissionIntensity = 2;
        public AnimationCurve colorFalloff;

        private readonly string _em = "_EmissionColor";
        private readonly string _alb = "_Color";
        private Color goalColor;
        private Color pulseColor;

        public Color flatColor => pulseColor / intensity;

        protected override void Start() {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            List<Material> mats = new List<Material>();
            mr.GetMaterials(mats);

            mat = mats.Count > 0 ? mats[0] : null;
            base.Start();

            lights = GetComponentsInChildren<Light>().ToList();
            lights.AddRange(GetComponents<Light>());
        }

        void Update() {
            if (mat) {
                mat.SetColor(_em, pulseColor * intensityMul);
                mat.SetColor(_alb, pulseColor);
            }

            foreach (Light l in lights) {
                l.color = flatColor;
                l.intensity = intensity * intensityMul;
            }

            if (SongController.instance.IsPlaying) {
                fallTime += Time.deltaTime;
                //intensity = Mathf.Lerp(silentEmissionIntensity, goalIntensity,
                //    colorFalloff.Evaluate(Mathf.Clamp01(fallTime / duration)));
                pulseColor = Color.Lerp(silentEmissionColor, goalColor,
                        colorFalloff.Evaluate(Mathf.Clamp01(fallTime / duration)));
                Vector3 s = Vector3.Lerp(initialScale, initialScale * scaleAmplitude,
                    scaleFalloff.Evaluate(Mathf.Clamp01(fallTime / duration)));
                transform.localScale = s;
            } else {
                pulseColor = Color.Lerp(pulseColor,
                    silentEmissionColor, speed * Time.deltaTime);
                Vector3 s = Vector3.Lerp(initialScale, transform.localScale,
                    Time.deltaTime * killSpeed);
                transform.localScale = s;
            }
        }

        public override void MajorNote() {
            goalColor = majorEmissionColor;
            //goalIntensity = majorEmissionIntensity;
            base.MajorNote();
        }

        public override void MinorNote() {
            goalColor = minorEmissionColor;
            //goalIntensity = minorEmissionIntensity;
            base.MajorNote();
        }
    }
}