using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace Krypt {
    public class ActionRecorder : MonoBehaviour {
        public static ActionRecorder instance;

        public bool recording = false;
        public Quantize resolution = Quantize.Eigth;
        
        // control
        private Transform ActivePlatform => EnemySpawner.instance.GetPlatform(platformIndex);
        private Transform ActiveTarget {
            get {
                switch(targetIndex){
                    case 0: return KryptController.instance.leftPlatform;
                    case 1: return KryptController.instance.middlePlatform;
                    default: return KryptController.instance.rightPlatform;
                }
            }
        }

        private Vector2 platformIndex;
        private int targetIndex;
        private string activeEnemy;
        private Enemy activeEnemyTemplate;

        [Header("Display")]
        public bool displayEnabled = true;
        public Transform spawnIndicator;
        public GameObject spawnIndGhost;
        public Transform targetIndicator;
        private Transform display;

        [Header("Save Data")]
        public string songFolder = "Krypt/Scripts/Songs";
        private string fileName => Application.dataPath + "/" + songFolder + "/" + (SongController.instance.currentSong != null 
            ? SongController.instance.currentSong.name : "NewSong") + ".json";
        private List<NoteEvent> eventList;

        [Header("Debug")]
        public Vector2 Location;
        private void Awake() {
            if (instance != null) {
                Destroy(this);
                return;
            }

            instance = this;
        }

        void Start() {

            display = transform.Find("Display");
            ToggleDisplay();
            SetIndicatorLoc();
            SetTargetLoc();
            LoadEvents();
        }

        void SetIndicatorLoc() {
            if (!displayEnabled) return;
            spawnIndicator.position = ActivePlatform.position;
        }

        void SetTargetLoc() {
            if (!displayEnabled) return;
            targetIndicator.position = ActiveTarget.position;
            spawnIndGhost.SetActive(false);
        }

        public void ToggleDisplay() {
            display.gameObject.SetActive(displayEnabled);
        }

        void LoadEvents() {
            if(SongController.instance.currentSong != null) {
                eventList = SongController.instance.currentSong.notes;
            } else {
                eventList = new List<NoteEvent>();
            }

            if (File.Exists(fileName)) {
                var dat = File.ReadAllText(fileName);
                var loaded = (List<NoteEvent>)JsonUtility.FromJson(dat, typeof(List<NoteEvent>));
                eventList.AddRange(loaded);
            }
        } 

        void SaveEvents() {

            if (!File.Exists(fileName)) {
                string dat = JsonUtility.ToJson(eventList);
                File.WriteAllText(fileName, dat);
            }
        }

        public void OnClickEnemy(Enemy enemy) {
            if (enemy.isStatic) {
                // need to create a new enemy of matching type on Spawn Event
                activeEnemyTemplate = enemy;
            } else {
                activeEnemy = enemy.id;
            }
        }

        public void OnMouseEnterPlatform(Transform t) {
            if (!displayEnabled) return;
            spawnIndGhost.SetActive(true);
            spawnIndGhost.transform.position = t.position;
        }
        public void OnMouseExitPlatform() {
            if (!displayEnabled) return;
            spawnIndGhost.SetActive(false);
        }

        public void OnClickPlatform(Transform t) {
            for (int y = 0; y < EnemySpawner.instance.platformMap.Count; y++) {
                for(int x = 0; x < EnemySpawner.instance.platformMap[y].Count; x++) {
                    if (EnemySpawner.instance.GetPlatform(x, y) == t) {
                        platformIndex = new Vector2(x, y);
                        SetIndicatorLoc();
                        return;
                    }
                }
            }
        }


        void Update() {
            Location.x = platformIndex.x;
            Location.y = platformIndex.y;

            // targets input
            var prev = new Vector2(platformIndex.x, platformIndex.y);
            if (Input.GetKeyDown(KeyCode.LeftArrow) && platformIndex.x > 0)
                platformIndex.x -= 1;
            if (Input.GetKeyDown(KeyCode.RightArrow) && platformIndex.x < EnemySpawner.instance.platformMap[(int)platformIndex.y].Count - 1) 
                platformIndex.x += 1;
            if (Input.GetKeyDown(KeyCode.UpArrow) && platformIndex.y < EnemySpawner.instance.platformMap.Count - 1) {
                platformIndex.y += 1;
                if (platformIndex.x >= EnemySpawner.instance.platformMap[(int)platformIndex.y].Count)
                    platformIndex.x = EnemySpawner.instance.platformMap[(int)platformIndex.y].Count - 1;
            } if (Input.GetKeyDown(KeyCode.DownArrow) && platformIndex.y > 0) {
                platformIndex.y -= 1;
                if (platformIndex.x >= EnemySpawner.instance.platformMap[(int)platformIndex.y].Count)
                    platformIndex.x = EnemySpawner.instance.platformMap[(int)platformIndex.y].Count - 1;
            }
            if (prev.x != platformIndex.x || prev.y != platformIndex.y)
                SetIndicatorLoc();

            var lastTarget = targetIndex;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                targetIndex = 0;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                targetIndex = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                targetIndex = 2;
            if (lastTarget != targetIndex)
                SetTargetLoc();

            // visual response
            if (Input.GetKeyDown(KeyCode.S))
                UiController.instance.OnKey(AttackType.Quick, true);
            if (Input.GetKeyDown(KeyCode.A))
                UiController.instance.OnKey(AttackType.Heavy, true);
            if (Input.GetKeyDown(KeyCode.D))
                UiController.instance.OnKey(AttackType.Projectile, true);
            if (Input.GetKeyDown(KeyCode.W))
                UiController.instance.OnKey(AttackType.Spawn, true);
            if (Input.GetKeyDown(KeyCode.Q))
                UiController.instance.OnKey(AttackType.Despawn, true);
            if (Input.GetKeyDown(KeyCode.E))
                UiController.instance.OnKey(AttackType.Warp, true);
            if (Input.GetKeyDown(KeyCode.LeftShift))
                UiController.instance.OnKey(AttackType.BlockOn, true);
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                UiController.instance.OnKey(AttackType.BlockOff, false);
            if (Input.GetKeyUp(KeyCode.S))
                UiController.instance.OnKey(AttackType.Quick, false);
            if (Input.GetKeyUp(KeyCode.A))
                UiController.instance.OnKey(AttackType.Heavy, false);
            if (Input.GetKeyUp(KeyCode.D))
                UiController.instance.OnKey(AttackType.Projectile, false);
            if (Input.GetKeyUp(KeyCode.W))
                UiController.instance.OnKey(AttackType.Spawn, false);
            if (Input.GetKeyUp(KeyCode.Q))
                UiController.instance.OnKey(AttackType.Despawn, false);
            if (Input.GetKeyUp(KeyCode.E))
                UiController.instance.OnKey(AttackType.Warp, false);


            if (recording) {
                if (Input.GetKeyDown(KeyCode.R)) {
                    recording = false;
                    spawnIndGhost.SetActive(false);
                } else {
                    // enemy action events
                    if (SongController.instance.IsPlaying) {
                        if (Input.GetKeyDown(KeyCode.S))
                            CreateNoteEvent(AttackType.Quick);
                        if (Input.GetKeyDown(KeyCode.A))
                            CreateNoteEvent(AttackType.Heavy);
                        if (Input.GetKeyDown(KeyCode.D))
                            CreateNoteEvent(AttackType.Projectile);
                        if (Input.GetKeyDown(KeyCode.W))
                            CreateNoteEvent(AttackType.Spawn);
                        if (Input.GetKeyDown(KeyCode.Q))
                            CreateNoteEvent(AttackType.Despawn);
                        if (Input.GetKeyDown(KeyCode.E))
                            CreateNoteEvent(AttackType.Warp);
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                            CreateNoteEvent(AttackType.BlockOn);
                        else if (Input.GetKeyUp(KeyCode.LeftShift)) 
                            CreateNoteEvent(AttackType.BlockOff);
                    }
                }
            } else {
                if (Input.GetKeyDown(KeyCode.R)) {
                    recording = true;
                }
            }
        }

        void CreateNoteEvent(AttackType type) {
            float t0 = SongController.instance.PlayTime;
            // event time quantization
            float rate = Mathf.Pow(2, (int)resolution) * 60 / SongController.instance.currentSong.BPM;
            t0 = rate*Mathf.Floor(t0 / rate);

            var evt = new NoteEvent(activeEnemy, t0,
                (int)platformIndex.x, (int)platformIndex.y, type);
            eventList.Add(evt);
        }
    }
}