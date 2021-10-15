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
        protected Rigidbody2D body;

        SpriteRenderer spriteRenderer;
        public float lifespan = -1.0f;

        void Awake()
        { 
            spriteRenderer = GetComponent<SpriteRenderer>();
            body = GetComponent<Rigidbody2D>();

            if (lifespan > 0.0f)
            {
                Destroy(gameObject, lifespan);
            }
        }

        protected void FixedUpdate()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, body.velocity.normalized);
            transform.Rotate(Vector3.forward, -90);
            if (body.velocity.x > 0)
            {
                spriteRenderer.flipY = true;
            }
        } 
    }
}