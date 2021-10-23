using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using UnityEngine.SceneManagement;
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

        public float fastFallForce = 8.0f;

        private JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        private Collider2D collider2d;
        private AudioSource audioSource;
        protected Health health;
        private bool controlEnabled = true;

        
        private GameObject gun;
        public GameObject[] guns;

        private GameObject melee;
        public GameObject[] melees;

        Vector2 worldcoord;

        bool jump;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        public int gunIndex = 0;
        public int meleeIndex = 0;

        private bool meleeEquipped = false;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            hud = GameObject.Find("Hud").GetComponent<HUDController>();
            hud.UpdateHUD(this);
            gun = guns[gunIndex];
            melee = melees[meleeIndex];
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

                if (Time.deltaTime > 0)
                {
                    Vector2 worldcoord;
                    worldcoord.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                    worldcoord.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

                    if (worldcoord.x > transform.position.x)
                    {
                        spriteRenderer.flipX = false;
                    }
                    else
                    {
                        spriteRenderer.flipX = true;
                    }
                }

                if (Input.mouseScrollDelta.y != 0)
                {
                    if (meleeEquipped)
                    {
                        melee.SetActive(false);
                        meleeIndex = (meleeIndex + (Input.mouseScrollDelta.y < 0 ? 1 : melees.Length - 1)) % melees.Length;
                        melee = melees[meleeIndex];
                        melee.SetActive(true);
                    }
                    else
                    {
                        gun.SetActive(false);
                        gunIndex = (gunIndex + (Input.mouseScrollDelta.y < 0 ? 1 : guns.Length - 1)) % guns.Length;
                        gun = guns[gunIndex];
                        gun.SetActive(true);
                    }
                    hud.UpdateHUD(this);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    meleeEquipped = false;
                    gun.SetActive(true);
                    melee.SetActive(false);
                } else if (Input.GetMouseButtonDown(1))
                {
                    meleeEquipped = true;
                    melee.SetActive(true);
                    gun.SetActive(false);
                }
            }
            UpdateJumpState();
            if (jump && isGrounded())
            {
                body.AddForce(new Vector2(0, jumpTakeOffSpeed * model.jumpModifier), ForceMode2D.Impulse);
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
            }

            animator.SetBool("grounded", isGrounded());
            animator.SetFloat("velocityX", Mathf.Abs(body.velocity.x) / maxSpeed);
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
                        melee.GetComponent<GunController>().RefillAmmo();
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

        public void setGun(int index)
        {
            if (index != gunIndex)
            {
                gun.SetActive(false);
                gunIndex = index;
                gun = guns[gunIndex];
                gun.SetActive(true);
                hud.UpdateHUD(this);
            }
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

        public void Bounce(Vector2 dir)
        {
            body.AddForce(dir, ForceMode2D.Impulse);
        }

        protected virtual void FixedUpdate()
        {
            if (controlEnabled)
            {
                float xMover = Input.GetAxis("Horizontal");
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

                float yMover = Input.GetAxis("Vertical");
                if (yMover < -0.1f)
                {
                    body.AddForce(new Vector2(0.0f, yMover * fastFallForce), ForceMode2D.Force);
                }
            }
        }

        public void Spawn()
        {
            collider2d.enabled = true;
            EnableControl(false);
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
                EnableControl(false);

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
            (meleeEquipped ? melee : gun).GetComponent<GunController>().setAlive(enabled);
        }
    }
}