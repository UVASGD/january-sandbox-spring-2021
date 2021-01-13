using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Krypt {
    public enum EnemyType { Minion, Soldier, BigDaddy, }


    public class Enemy : MonoBehaviour {

        // Attacks that require a platform location
        private static AttackType[] locTypes = { AttackType.Spawn, AttackType.Warp };
        // Attacks that are aimed at player
        private static AttackType[] targTypes = { AttackType.Quick, AttackType.Heavy, AttackType.Projectile };
        public bool TypeIsLoc(AttackType type) { return Array.Exists(locTypes, t => t == type); }
        public bool TypeIsTarg(AttackType type) { return Array.Exists(targTypes, t => t == type); }


        [Header("Information")]
        public bool isStatic;
        public string id;
        public EnemyType classification;

        [Header("Customization")]
        public int maxHealth;
        public int attackStrength;
        public Dictionary<AttackType, GameObject> attackFX;

        [Header("Scene Refs")]
        public Animator anim;

        /// <summary>
        /// Events performed by this Enemy on specific beats
        /// </summary>
        private List<NoteEvent> actions;
        private Transform fxLocation;
        private bool blocking;
        private Player player;
        private int health;

        private void Awake() {
            actions = new List<NoteEvent>();
        }
        public void LoadActions(List<NoteEvent> newActions) {
            actions.AddRange(newActions);
        }

        private void Start() {
            anim = transform.GetComponentInChildren<Animator>();
            fxLocation = transform.Find("Attack Location");
            player = KryptController.instance.player;
            health = maxHealth;

            Squasher sq = GetComponent<Squasher>();
            sq.disabled = isStatic;
        }

        void ReadEvent(NoteEvent evt) {
            Vector3 target = transform.position;
            if(TypeIsLoc(evt._type)) {
                Transform platform = EnemySpawner.instance.GetPlatform(evt._lineIndex, evt._lineLayer);
                if(platform == null) {
                    Debug.LogWarning("Could not read " + evt + " because the given platform was not found.");
                    return;
                }

                target = platform.position;
            } if (TypeIsTarg(evt._type)) {
                target = player.transform.position;
            }

            switch (evt._type) {
                case AttackType.BlockOn:
                    anim.SetBool("Blocking", blocking = true);
                    break;
                case AttackType.BlockOff:
                    anim.SetBool("Blocking", blocking = false);
                    break;
                case AttackType.Warp:
                    break;
                case AttackType.Quick:
                case AttackType.Heavy:
                case AttackType.Projectile:
                    PerformAction(evt._type, target);
                    break;
            }

        }

        void PerformAction(AttackType type, Vector3 target) {
            anim.SetInteger("AttackType", (int)type);
            anim.SetTrigger("Attack");
            var sfx = attackFX[type];
            if(sfx != null) {
                // calculate velocity from BPM * speed of attack
                Vector3 distance = target - fxLocation.position;
                // calculate direction from target and position

                var sfx_go = SFX_Spawner.instance.SpawnFX(sfx, fxLocation.position);
                //var attack = sfx_go.GetComponent<AttackEffect>();
                //attack.owner = this;
                //attack.type = type;
            }
        }

        public void OnHit(int attackStrength) {
            if (blocking) return;
            health -= attackStrength;
            if (health <= 0) {
                anim.SetTrigger("Die");
            } else {
                anim.SetTrigger("Recoil");
            }
        }

        void Update() {
            if (isStatic || !SongController.instance.IsPlaying) return;

        }


    }
}