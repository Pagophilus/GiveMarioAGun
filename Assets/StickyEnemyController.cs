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
    public class StickyEnemyController : EnemyController
    {
        private Rigidbody2D rb;
        private CircleCollider2D bc;

        private RaycastHit2D hit;
        private RaycastHit2D downHit;

        private bool isMove;

        private Vector2 dir;
        private float random;
        private float newDir;
        private float turnCountdown = 0.0f;

        public float rayDist = 0.4f;

        public Transform obj;

        public float rotateSpeed = 50;
        public float moveSpeed = 5;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            bc = GetComponent<CircleCollider2D>();

            random = Random.Range(-1, 2);
            rb.velocity = new Vector2(3, 0);
            if (random > 0)
            {

                newDir = 1;
            }
            else
            {
                newDir = -1;
            }
        }

        void Update()
        {
            turnCountdown-= Time.deltaTime;
            dir = transform.right * 1.0f;
            hit = Physics2D.Raycast(transform.position, dir, rayDist);
            downHit = Physics2D.CircleCast(transform.position, bc.radius, -transform.up, 1 * rayDist);
            //downHit = Physics2D.Raycast(transform.position, transform.right - transform.up, 5 * rayDist);

            if (hit && turnCountdown < 0.0f)
            {
            turnCountdown = 0.1f;
                transform.rotation *= Quaternion.Euler(0f, 0f, 90.0f);
                rb.velocity = Quaternion.Euler(0f, 0f, 90.0f) * rb.velocity;
            }
            if (!downHit && turnCountdown < 0.0f)
            {
                turnCountdown = 0.1f;
                transform.rotation *= Quaternion.Euler(0f, 0f, -90.0f);
                rb.velocity = Quaternion.Euler(0f, 0f, -90.0f) * rb.velocity;
            }

            /*if (dir == (Vector2)transform.right)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }*/

            /*if (downHit)
            {
               //Debug.Log("ddd downHit ")
                isMove = true;
                transform.up = (Vector2)transform.position - downHit.point;
                if (hit)
                {
                    obj = hit.transform;
                    transform.RotateAround(hit.transform.position, transform.forward, rotateSpeed * newDir * Time.deltaTime);

                }
                else
                {
                    obj = null;
                }
            }
            else
            {
                if (obj != null)
                {
                    transform.RotateAround(obj.position, transform.forward, rotateSpeed * newDir * Time.deltaTime);
                }
            }
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);*/
        }

        private void FixedUpdate()
        {
            /*if (isMove)
            {
                if (downHit)
                {
                    rb.velocity = rb.velocity + (Vector2)transform.up * -2f; // needs work
                    rb.velocity = rb.velocity + dir * moveSpeed;
                }
            }
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -2f, 2f), Mathf.Clamp(rb.velocity.y, -2f, 2));
            */
            //rb.position = (rb.position + Time.deltaTime * rb.velocity);

        }
    }
}