using UnityEngine;
using System;
using System.Collections.Generic;

namespace Krypt {

    public enum AttackType { Quick, Heavy, Block, Projectile }

    public enum Difficulty { Easy, Normal, Hard }

    [CreateAssetMenu(fileName = "New Song", menuName = "Krypt/Song")]
    public class SongData : ScriptableObject {
        [Header("General")]
        public string title;
        public AudioClip song;

        [Header("Timing")]
        [Range(1, 400)] public int BPM;
        [Tooltip("Number if beats in a measure")]
        public int beatsPerBar = 4;
        public int measureUnit = 4;
        [Tooltip("The number of measures to wait before playing")]
        public int pickupCount = 2;
        public List<NoteEvent> notes;
    }

    [Serializable]
    public class NoteEvent {
        public float _time;
        public int _lineIndex;
        public int _lineLayer;
        public AttackType _type;
        public NoteEvent(float time, int column, int row, AttackType type) {
            _time = time;
            _lineIndex = column;
            _lineLayer = row;
            _type = type;
        }
    }
}