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
    public class MeleeController : MonoBehaviour
    {

        public GameObject explosion;
        void Awake()
        {
        }


        protected void FixedUpdate()
        {
        }

        protected void ComputeVelocity()
        {
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("ddd1 sword collided " + collision.gameObject.name);
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(10);
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                }
            }
            if (explosion != null)
            {
                Instantiate(explosion, (Vector2)transform.position, Quaternion.identity);
            }
        }
    }
}