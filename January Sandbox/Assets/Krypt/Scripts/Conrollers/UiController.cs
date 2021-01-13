using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Krypt {
    public class UiController : MonoBehaviour {

        public static UiController instance;

        [Header("Timing")]
        public Image btnPlay;
        public Sprite pause;
        public Sprite play;
        public Image btnRec;
        public Color recordingColor;

        [Header("Action Recording")]
        public Color highlightColor;
        public Color normalColor;
        public Image btnNormal;
        public Image btnSlow;
        public Image btnFast;
        public Image btnWarp;
        public Image btnSpawn;
        public Image btnDespawn;
        public Image btnBlock;


        private void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
        }

        private void Update() {
            btnPlay.sprite = SongController.instance.CanPlay ? play : pause;
            btnRec.color = ActionRecorder.instance.recording ? recordingColor : normalColor;
        }

        public void PlayClick() {
            if (SongController.instance.CanPlay)
                SongController.instance.Continue();
            else
                SongController.instance.Pause();
        }

        public void OnKey(AttackType type, bool down) {
            Image btn = null;

            switch (type) {
                case AttackType.BlockOff:
                case AttackType.BlockOn:
                    btn = btnBlock; break;
                case AttackType.Despawn:
                    btn = btnDespawn; break;
                case AttackType.Heavy:
                    btn = btnSlow; break;
                case AttackType.Projectile:
                    btn = btnFast; break;
                case AttackType.Quick:
                    btn = btnNormal; break;
                case AttackType.Spawn:
                    btn = btnSpawn; break;
                case AttackType.Warp:
                    btn = btnWarp; break;
            }

            if(btn != null) {
                btn.color = down ? highlightColor : normalColor;
            }
        }

        public void ResetClick() {
            if (SongController.instance.IsPlaying) SongController.instance.Pause();
            SongController.instance.Begin();
        }
    }
}