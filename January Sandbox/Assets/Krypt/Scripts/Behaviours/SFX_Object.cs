using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Krypt {
    public class SFX_Object : MonoBehaviour {

        public float pitch_range = 0.2f, amp_range = 0.02f;
        public float vol = -1f;

        public List<AudioClip> clips = new List<AudioClip>();
        public AudioMixerGroup mixerGroup;

        // Use this for initialization
        void Start() {
            float max_audio_len = AdjustAudio(), max_part_len = 0;
            foreach (ParticleSystem part in GetComponentsInChildren<ParticleSystem>()) {
                if (part.main.duration > max_part_len) max_part_len = part.main.duration;
            }
            Destroy(gameObject, 1.5f * Mathf.Max(max_audio_len, max_part_len));
        }

        protected float AdjustAudio() {
            float max_audio_len = 0;
            foreach (AudioSource aud in GetComponents<AudioSource>()) {
                if (vol != -1) {
                    aud.volume = vol;
                }
                aud.outputAudioMixerGroup = mixerGroup;
                aud.pitch += Random.Range(-pitch_range, pitch_range);
                aud.volume += Random.Range(-amp_range, 0);
                if (aud.clip.length > max_audio_len) max_audio_len = aud.clip.length;
            }
            return max_audio_len;
        }
    }
}