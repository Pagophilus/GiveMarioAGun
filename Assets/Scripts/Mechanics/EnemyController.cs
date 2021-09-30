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
        public float damage = 10.0f;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        private TimerController timer;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            timer = GameObject.Find("Timer").GetComponent<TimerController>();
            damage = 10.0f;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //Debug.Log("Collided");
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("PlayerCollided " + collision.gameObject.name);
                Vector2 dist = player.transform.position - transform.position;
                dist.Normalize();
                player.Bounce(0.8f * dist);
                //player.Bounce(0.8f);

                //player.GetComponent<Rigidbody2D>().AddForce()
                timer.Damage(damage);
            }
        }

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
        }

    }
}