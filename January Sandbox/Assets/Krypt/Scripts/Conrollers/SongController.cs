using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Krypt {

    public delegate void MusicEvent();


    //public delegate void TrackingEvent(TrackType type, Vector3 pos);

    public enum Quantize {
        None = -8,
        SixtyFourths = -4,
        ThirtySecond = -3,
        Sixteenth = -2,
        Eigth = -1,
        Quarter = 0,
        Half = 1,
        Whole = 2,
        Doulble = 3,
    }

    public enum MetronomeType { Off, Pickup, On }

    public class SongController : MonoBehaviour {

        public static SongController instance;

        public SongData currentSong;

        [Header("Timing")]
        [Tooltip("The number of frames to offset a rhythmic pulse")]
        public int pulseFrameOffset = 2;
        [Tooltip("Number of Eigth notes to offset the measure")]
        public int beatOffset = 1;
        private float _off_b => beatOffset * MinorTime / 2;

        [Header("Metronome")]
        public MetronomeType metronomeEnabled = MetronomeType.Pickup;
        [Tooltip("Object in scene representing the metronome")]
        public GameObject MetronomeObj;
        [Tooltip("Prefab SFX_Obj for full measure tick")]
        public GameObject metronomeMajor;
        [Tooltip("Prefab SFX_Obj for beat tick")]
        public GameObject metronomeMinor;

        [Header("Song Info")]
        public Difficulty difficulty;

        private AudioSource src;

        /// <summary> Interval time for a single measure </summary>
        public float MajorTime => (currentSong != null ? currentSong.beatsPerBar : 0) * A * P / playSpeed;
        /// <summary> Interval time for a single beat in measure </summary>
        public float MinorTime => A * P / playSpeed;
        float P => 1f / (currentSong != null ? currentSong.BPM : 120);
        readonly float A = 60; // generally converts to FPS

        public Quantize quantize = Quantize.Eigth;
        public bool IsPlaying { get; private set; }
        public bool CanPlay => !IsPlaying && playInitiater == null;

        [Range(.25f, 2)] public float playSpeed = 1;

        public MusicEvent MajorNote;
        public MusicEvent MinorNote;
        public MusicEvent OffbeatNote;

        public MusicEvent OnPauseSong;
        public MusicEvent OnPlaySong;
        public MusicEvent OnRelocateTime;

        /// <summary> Coroutine that delays playing the song from start </summary>
        private Coroutine playInitiater;
        private float delayTime;
        //private float DelayTime => (float)(DateTime.Now - delayStartTime).TotalSeconds;
        private float PickupTime => MajorTime * (currentSong != null ? currentSong.pickupCount : 2);

        private bool init = false;
        public float PlayTime { get; private set; }

        private void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
            Init();
        }

        private void Init() {
            src = GetComponent<AudioSource>();
            if (src.loop) src.loop = false;
        }

        void FixedUpdate() {
            if (IsPlaying) {
                // check if the song has finished
                if (!src.isPlaying) {
                    Stop();
                    return;
                }

                PlayTime += Time.deltaTime;
                PulseCheck(PlayTime);

                if (!src.isPlaying) {
                    PlayTime = 0;
                    Pause();
                }
            } else if (playInitiater != null) {
                delayTime += Time.deltaTime;
                PulseCheck(delayTime);
            }
        }

        /// <summary>
        /// Checks if a rythmic pulse can be applied at the given time in seconds
        /// </summary>
        private void PulseCheck(float t0) {
            bool doTick = metronomeEnabled == MetronomeType.On ||
                (metronomeEnabled == MetronomeType.Pickup && !IsPlaying && !CanPlay);

            if ((t0 + pulseFrameOffset * Time.deltaTime + _off_b) % MajorTime <= Time.deltaTime && metronomeMajor) {
                if (doTick) SFX_Spawner.instance.SpawnFX(metronomeMajor, MetronomeObj.transform.position,
                     parent: MetronomeObj.transform);
                MajorNote?.Invoke();
            } else if ((t0 + pulseFrameOffset * Time.deltaTime + _off_b) % MinorTime <= Time.deltaTime && metronomeMinor) {
                if (doTick) SFX_Spawner.instance.SpawnFX(metronomeMinor, MetronomeObj.transform.position,
                    parent: MetronomeObj.transform);
                MinorNote?.Invoke();
            } else if ((t0 + pulseFrameOffset * Time.deltaTime) % MinorTime <= Time.deltaTime && metronomeMinor) {
                OffbeatNote?.Invoke();
            }
        }

        #region Play Controls
        /// <summary>
        /// Start playback information. Resets all previous data
        /// </summary>
        public void Begin() {
            init = true;
            src.time = PlayTime = 0;
            playInitiater = StartCoroutine(PlayDelay());
        }

        /// <summary>
        /// pauses playback
        /// </summary>
        public void Pause() {
            if (playInitiater != null) {
                // Paused During pickup
                StopCoroutine(playInitiater);
                playInitiater = null;
            } else {
                // Paused during song
                IsPlaying = false;
                src.Pause();
                //ConveyorController.instance.Pause();
                OnPauseSong?.Invoke();

                // quantize play time to the nearest measure
                PlayTime -= (PlayTime % MajorTime);
                src.time = PlayTime;
            }
        }

        /// <summary>
        /// continues playback at a quantized interval behind the current trackTime
        /// </summary>
        public void Continue() {
            if (!init) {
                Begin();
                return;
            }

            playInitiater = StartCoroutine(PlayDelay());
        }

        public void Stop() {
            IsPlaying = false;
            src.Stop();
            src.time = PlayTime = 0;
            init = false;
            OnPauseSong?.Invoke();
        }

        /// <summary>
        /// Start playback after a delay
        /// </summary>
        /// <returns></returns>
        public IEnumerator PlayDelay() {
            delayTime = 0;
            yield return new WaitForSeconds(PickupTime);

            IsPlaying = true;
            OnPlaySong?.Invoke();
            src.Play();
            playInitiater = null;
        }

        #endregion


    }
}
