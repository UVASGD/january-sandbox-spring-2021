using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Krypt {
    public class UiController : MonoBehaviour {

        public static UiController instance;

        public TextMeshProUGUI btnPlay;

        private void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
        }

        private void Update() {
            btnPlay.text = SongController.instance.CanPlay ? "Play" : "Pause";
        }

        public void PlayClick() {
            if (SongController.instance.CanPlay)
                SongController.instance.Continue();
            else
                SongController.instance.Pause();
        }

        public void ResetClick() {
            if (SongController.instance.IsPlaying) SongController.instance.Pause();
            SongController.instance.Begin();
        }
    }
}