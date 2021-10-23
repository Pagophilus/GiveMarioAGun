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
        public bool ricochet = false;
        public int damage;

        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("ddd hit wall ");
            //collision.gameObject.GetComponent<EnemyController>();
            var enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damage);
            }         
            else
            {
                var button = collision.gameObject.GetComponent<ButtonController>();
                if (button != null)
                {
                    button.Activate();
                }
            }
            if (ricochet)
            {
                GetComponent<Rigidbody2D>().AddForce(collision.contacts[0].normal * 15.0f, ForceMode2D.Impulse);
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

        void OnTriggerEnter2D(Collider2D collision)
        {
            //collision.gameObject.GetComponent<EnemyController>();
            var enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damage);
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