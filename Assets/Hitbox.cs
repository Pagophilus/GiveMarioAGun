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
    public class Hitbox : MonoBehaviour
    {
        public GameObject explosion;
        public bool destroyOnContact;
        public int damage;

        void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                var enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(damage);
                }
                else
                {
                    Schedule<EnemyDeath>().enemy = enemy;
                }
            }
            else
            {
                var button = collision.gameObject.GetComponent<ButtonController>();
                if (button != null)
                {
                    button.Activate();
                }
            }
            if (explosion != null)
            {
                Instantiate(explosion, (Vector2)transform.position, Quaternion.identity);
            }
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }
}