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
        //public Vector2 velocity;
        protected Rigidbody2D body;
        //HUD Stuff--------------------------------------------
        private HUDController hud;

        //Slam Animation
        private float pastYVel = 0.0f;
        public GameObject slamObject;
  
        //LayerMasks
        [SerializeField] private LayerMask layerMask;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        private JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        private Collider2D collider2d;
        private AudioSource audioSource;
        protected Health health;
        private bool controlEnabled = true;

        private GameObject crosshair;
        private GameObject gun;

        public GameObject[] guns;

        float xMover;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        public int gunIndex = 0;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            hud = GameObject.Find("Hud").GetComponent<HUDController>();
            crosshair = GameObject.Find("Donut");
            hud.UpdateHUD(this);
            gun = guns[gunIndex];
        }

        protected void Update()
        {            
            if (controlEnabled)
            {
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

                if (Input.GetMouseButtonDown(1))
                {
                    gun.SetActive(false);
                    gunIndex = (gunIndex + 1) % guns.Length;
                    gun = guns[gunIndex];
                    gun.SetActive(true);
                    hud.UpdateHUD(this);
                }

                //Gun animation
                gun.GetComponentInChildren<SpriteRenderer>().flipY = (worldcoord.x > transform.position.x);
                gun.transform.rotation = Quaternion.Euler(0, 0, (worldcoord.x > transform.position.x ? 180 : 0) + 90 + 180 / Mathf.PI * (Mathf.Atan((transform.position.y - worldcoord.y) / (transform.position.x - worldcoord.x))));                          
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            ComputeVelocity();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.Grounded:
                    if (!isGrounded())
                    {
                        if (audioSource && jumpAudio)
                            audioSource.PlayOneShot(jumpAudio);
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!isGrounded())
                    {
                        if (audioSource && jumpAudio)
                            audioSource.PlayOneShot(jumpAudio);
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (isGrounded())
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                        gun.GetComponent<GunController>().RefillAmmo();
                        //Slam animation
                        Instantiate(slamObject, (Vector2)transform.position, Quaternion.identity);
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
                body.AddForce(new Vector2(0, jumpTakeOffSpeed * model.jumpModifier), ForceMode2D.Impulse);
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                /*if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }*/
            }

            if (xMover > 0.01f) 
                spriteRenderer.flipX = false;
            else if (xMover < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", isGrounded());
            animator.SetFloat("velocityX", Mathf.Abs(body.velocity.x) / maxSpeed);
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
            var hit = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.2f, layerMask).collider;
           // Debug.Log(hit.gameObject.name);
            return (hit != null);
        }

        //--------------------------------------------------------------
        public void Teleport(Vector3 position)
        {
            body.position = position;
            body.velocity *= 0;
        }

        public void Bounce(float value)
        {
            body.AddForce(new Vector2(0.0f, value), ForceMode2D.Impulse);
        }

        protected virtual void FixedUpdate()
        {
            if (controlEnabled)
            {
                xMover = Input.GetAxis("Horizontal");
                if (body.velocity.x > -maxSpeed && xMover < -0.1f)
                {
                    body.AddForce(new Vector2(-1 * maxSpeed, 0.0f), ForceMode2D.Force);
                }
                else if (body.velocity.x < maxSpeed && xMover > 0.1f)
                {
                    body.AddForce(new Vector2(1 * maxSpeed, 0.0f), ForceMode2D.Force);
                }
                else if (Math.Abs(xMover) < 0.1f)
                {
                    body.AddForce(new Vector2(body.velocity.x / -2.0f, 0.0f), ForceMode2D.Force);
                }
            }
        }

        public void Spawn()
        {
            collider2d.enabled = true;
            controlEnabled = false;
            if (audioSource && respawnAudio)
                audioSource.PlayOneShot(respawnAudio);
            health.Increment();
            Teleport(model.spawnPoint.transform.position);
            jumpState = PlayerController.JumpState.Grounded;
            animator.SetBool("dead", false);
            model.virtualCamera.m_Follow = transform;
            model.virtualCamera.m_LookAt = transform;
            Simulation.Schedule<EnablePlayerInput>(2f);
        }

        public void Die()
        {
            if (health.IsAlive)
            {
                health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                controlEnabled = false;

                if (audioSource && ouchAudio)
                {
                    audioSource.PlayOneShot(ouchAudio);
                }
                animator.SetTrigger("hurt");
                animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }

        public void EnableControl(bool enabled)
        {
            controlEnabled = enabled;
        }
    }
}