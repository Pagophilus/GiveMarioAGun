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
    public class EnemyHitbox : MonoBehaviour
    {
        public GameObject explosion;
        public bool destroyOnContact;
        private TimerController timer;
        public int damage;

        void Awake() {
            timer = GameObject.Find("Timer").GetComponent<TimerController>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("PlayerCollided " + collision.gameObject.name);
                Vector2 dist = player.transform.position - transform.position;
                dist.Normalize();
                player.Bounce(0.8f * dist);
                timer.Damage(damage);
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

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("PlayerCollided " + collision.gameObject.name);
                Vector2 dist = player.transform.position - transform.position;
                dist.Normalize();
                player.Bounce(0.8f * dist);
                timer.Damage(damage);
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