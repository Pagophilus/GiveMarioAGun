using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class BulletController : MonoBehaviour
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;

        protected Rigidbody2D body;

        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        private Vector2 vStart;


        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
        }

/*        protected override void Start()
        {            velocity.y = vStart.y;
           targetVelocity.x = vStart.x;

        }*/

        public void setVelocity(Vector2 v)
        {
        }

        protected void Update()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, body.velocity.normalized);
            transform.Rotate(Vector3.forward, -90);
            if (body.velocity.x > 0)
            {
                spriteRenderer.flipY = true;
            }
        }

        protected void ComputeVelocity()
        {
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                }
            }
            Destroy(gameObject);
        }
    }
}