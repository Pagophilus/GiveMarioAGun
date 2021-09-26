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
    /// Class meant to control the bullet, its movement, and death in a ball of flame.
    /// </summary>
    public class BulletController : MonoBehaviour
    {

        private bool destroy;
        protected Rigidbody2D body;

        SpriteRenderer spriteRenderer;
        internal Animator animator;

        public GameObject explosion;
        void Awake()
        { 
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            destroy = false;
        }


        protected void FixedUpdate()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, body.velocity.normalized);
            transform.Rotate(Vector3.forward, -90);
            if (body.velocity.x > 0)
            {
                spriteRenderer.flipY = true;

            }
            if (destroy)
            {
                Destroy(gameObject);
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
            Debug.Log("HEY");
            if(explosion != null)
            Instantiate(explosion, (Vector2)transform.position, Quaternion.identity);
            destroy = true;
            
            
        }
    }
}