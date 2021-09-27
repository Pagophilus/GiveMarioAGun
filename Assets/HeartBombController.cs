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
    public class HeartBombController : MonoBehaviour
    {

        private bool destroy;
        protected Rigidbody2D body;

        SpriteRenderer spriteRenderer;
        internal Animator animator;

        public GameObject explosion;
        private float spin;
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            spin = Random.Range(-4.0f, 4.0f);
            destroy = false;
            transform.Rotate(Vector3.forward, spin);
        }


        protected void Update()
        {
            if (!body.isKinematic)
            {
                transform.Rotate(Vector3.forward, spin);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(explosion, (Vector2)transform.position, Quaternion.identity);
                Destroy(gameObject);
            }    
        }

        protected void ComputeVelocity()
        {
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("ddd heart collided " + collision.gameObject.name);
            body.isKinematic = true;
            gameObject.transform.SetParent(collision.gameObject.transform);
        }
    }
}