using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;
        public float launchSpeed = 12.0f;
        public GameObject bullet;
        public GameObject deathEffect;
        protected GameObject thePlayer;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        protected float fireDelay = 1.0f;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            thePlayer = GameObject.Find("Player");
        }

        public virtual void OnDamaged(int newHP, int oldHP) {
            if (oldHP > 0 && newHP == 0 && deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
        }

        protected virtual void Update()
        {
            if (fireDelay < 0.0f && Vector2.Distance(thePlayer.transform.position, transform.position) < 6.0f)
            {
                Vector2 fireAngle = thePlayer.transform.position - transform.position;
                fireAngle.Normalize();
                GameObject o = Instantiate(bullet, transform.position, Quaternion.identity);
                o.GetComponent<Rigidbody2D>().AddForce(fireAngle * launchSpeed, ForceMode2D.Impulse);
                fireDelay = 2.0f;
            }
            fireDelay -= Time.deltaTime;
            if (path != null)
            {
                if (mover == null)
                {
                    mover = path.CreateMover(control.maxSpeed * 0.5f);
                }
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
                control.move.y = Mathf.Clamp(mover.Position.y - transform.position.y, -1, 1);
            }
        }
    }
}