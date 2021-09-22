using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    /// 


    public class PlayerController : MonoBehaviour
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        
        //Shit from kinematic-------------------------
        public bool IsGrounded { get; private set; }
        protected Vector2 targetVelocity;
        public Vector2 velocity;
        protected Rigidbody2D body;
        //--------------------------------------------


        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        public GameObject crosshair;
        public GameObject gun;

        float xMover;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        public SpriteRenderer gunSprite;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected void Update()
        {
            
            if (controlEnabled)
            {
                /*float input = Input.GetAxis("Horizontal");
                if (input > 0.1f && velocity.x < maxSpeed)
                {
                    move.x = velocity.x + 0.02f;
                } else if (input < -0.1f && velocity.x > 0.0f - maxSpeed)
                {
                    move.x = velocity.x - 0.02f;
                }
                else
                {
                    move.x = 0;
                }*/
                xMover = Input.GetAxis("Horizontal");
                
                if(body.velocity.x > -maxSpeed && xMover < 0)
                {
                    body.AddForce(new Vector2(-1 * maxSpeed, .1f), ForceMode2D.Force);
                } else if (body.velocity.x < maxSpeed && xMover > 0)
                {
                    body.AddForce(new Vector2(1 * maxSpeed, .1f), ForceMode2D.Force);

                }


                // Debug.Log(move.x);
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                {
                    jumpState = JumpState.PrepareToJump;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                Vector2 worldcoord;
                worldcoord.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                worldcoord.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                crosshair.transform.position = worldcoord;

                gun.GetComponentInChildren<SpriteRenderer>().flipY = (worldcoord.x > transform.position.x);
                gun.transform.rotation = Quaternion.Euler(0, 0, (worldcoord.x > transform.position.x ? 180 : 0) + 90 + 180 / Mathf.PI * (Mathf.Atan((transform.position.y - worldcoord.y) / (transform.position.x - worldcoord.x))));

                            
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            ComputeVelocity();
            //base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!isGrounded())
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (isGrounded())
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected void ComputeVelocity()
        {
            if (jump && isGrounded())
            {
                body.AddForce(new Vector2(0,jumpTakeOffSpeed * model.jumpModifier),ForceMode2D.Impulse);
                //velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (xMover > 0.01f) 
                spriteRenderer.flipX = false;
            else if (xMover < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", isGrounded());
            animator.SetFloat("velocityX", Mathf.Abs(body.velocity.x) / maxSpeed);


                targetVelocity = move * maxSpeed;

           // Debug.Log("Y velocity???" + velocity.y);
           // Debug.Log(velocity);
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public bool isGrounded()
        {
            
            float distToGround = collider2d.bounds.extents.y;
            var hit = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.2f).collider;
            //Debug.Log(hit.gameObject.name);
            return (hit != null);
        }
        //--------------------------------------------------------------
        public void Teleport(Vector3 position)
        {
            body.position = position;
            velocity *= 0;
            body.velocity *= 0;
        }

        public void Bounce(float value)
        {
            velocity.y = value;
        }
        protected virtual void FixedUpdate()
        {
            //if already falling, fall faster than the jump speed, otherwise use normal gravity.
            //if (velocity.y < 0)
              //  velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            //else
             //   velocity += Physics2D.gravity * Time.deltaTime;

            velocity.x = targetVelocity.x;

            //IsGrounded = false;

            var deltaPosition = velocity * Time.deltaTime;

           // var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

           // var move = moveAlongGround * deltaPosition.x;

            PerformMovement(move, false);

           // move = Vector2.up * deltaPosition.y;

            PerformMovement(move, true);

        }
        void PerformMovement(Vector2 move, bool yMovement)
        {
           // var distance = move.magnitude;

           /* if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;

                    //is this surface flat enough to land on?
                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        // if moving up, change the groundNormal to new surface normal.
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    if (IsGrounded)
                    {
                        //how much of our velocity aligns with surface normal?
                        var projection = Vector2.Dot(velocity, currentNormal);
                        if (projection < 0)
                        {
                            //slower velocity if moving against the normal (up a hill).
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                    else
                    {
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        velocity.x *= 0;
                        velocity.y = Mathf.Min(velocity.y, 0);
                    }
                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }*/
           // body.position = body.position + move.normalized * distance;
        }
    }
}