using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Krypt {

    [CustomEditor(typeof(SongController))]
    public class SongController_Editor : Editor {

        SongController sc;
        GUILayoutOption h = GUILayout.Height(20);

        private void Awake() {
            sc = (SongController)target;
        }

        public override void OnInspectorGUI() {
            
            if (!sc.IsPlaying) {
                if (GUILayout.Button("Pway", h)) {
                    sc.Continue();
                }
            } else {
                if(GUILayout.Button("Paws", h)) {
                    sc.Pause();
                }
            }

            if(GUILayout.Button("Reset", h)) {
                if(sc.IsPlaying) sc.Pause();
                sc.Begin();
            }

            GUILayout.Space(20);

            base.OnInspectorGUI();
        }
    }
}