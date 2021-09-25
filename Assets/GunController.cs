using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Mechanics.BulletController;
using static Platformer.Mechanics.PlayerController;
using static Platformer.Mechanics.EnemyController;
using static Platformer.Gameplay.EnemyDeath;
using  Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class GunController : MonoBehaviour
    {
        public GameObject bullet;
        public GameObject impactFX;

        public int pellets = 5;
        public int burstCount = 1;
        public float burstGap = 0.25f;
        public float spread = 40.0f;
        public float recoil = 8.0f;
        public int magazineCap = 10;
        private int magazine = 10;

        private HUDController hud;

        /// <summary>
        /// Bullets per Minute
        ///  <summary> 
        public float fireRate = 4.0f;
        public bool auto = false;
        public bool hitscan = false;
        public bool continuous = false;
        public bool randomSpread = false;
        private LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline

        private float fireCountDown = -1.0f;

        // Start is called before the first frame update
        void Start()
        {
            laserLine = GetComponent<LineRenderer>();
            if (laserLine != null)
            {
                laserLine.sortingLayerName = "Foreground";
            }
            magazine = magazineCap;
            hud.UpdateAmmo(magazine, magazineCap);
        }

        void Awake()
        {
            hud = GameObject.Find("Hud").GetComponent<HUDController>();
        }

        void OnEnable()
        {
            hud.UpdateAmmo(magazine, magazineCap);
        }

        public void RefillAmmo()
        {
            magazine = magazineCap;
            hud.UpdateAmmo(magazine, magazineCap);
        }



        IEnumerator shoot()
        {
            //gameObject.GetComponentInParent<Rigidbody2D>().AddForce(transform.up * -1.0f * recoil, ForceMode2D.Impulse);
            magazine--;
            hud.UpdateAmmo(magazine, magazineCap);
            if (hitscan)
            {
                gameObject.GetComponentInParent<Rigidbody2D>().AddForce(transform.up * -1.0f * recoil, ForceMode2D.Impulse);

                // Declare a raycast hit to store information about what our raycast has hit
                RaycastHit2D hit;

                // Set the start position for our visual effect for our laser to the position of gunEnd
                if (laserLine != null)
                {
                    laserLine.SetPosition(0, transform.position + transform.up * 0.5f);
                }

                Debug.Log("getting hits");
                // Check if our raycast has hit anything
                hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, 100.0f);
                if (hit.collider != null)
                {
                    //Debug.Log("hit found: " + hit.point.x + " " + hit.point.y + " " + hit.collider.gameObject.name);

                    if (laserLine != null)
                    {
                        // Set the end position for our laser line 
                        laserLine.SetPosition(1, hit.point);
                    }

                    var enemy = hit.collider.gameObject.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        var enemyHealth = enemy.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemyHealth.Decrement();
                        }
                    }
                    Instantiate(impactFX, hit.point + (Vector2)transform.up * -0.02f, Quaternion.LookRotation(hit.normal));
                }
                else
                {
                    Debug.Log("no hit found: ");

                    if (laserLine != null)
                    {
                        // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                        laserLine.SetPosition(1, transform.position + transform.up * 50.0f);
                    }
                }
            }
            else
            {
                Vector2 fireAngle;
                GameObject o;

                for (int h = 0; h < burstCount; h++)
                {
                    gameObject.GetComponentInParent<Rigidbody2D>().AddForce(transform.up * -1.0f * recoil, ForceMode2D.Impulse);
                    for (int i = 0; i < pellets; i++)
                    {
                        fireAngle = Quaternion.Euler(0, 0, spread / 2.0f * (Random.value * 2 - 1)) * transform.up;
                        o = Instantiate(bullet, transform.position + transform.up * 0.5f, Quaternion.identity);
                        o.GetComponent<Rigidbody2D>().AddForce(fireAngle * (18 + 4 * Random.value), ForceMode2D.Impulse);
                    }
                    yield return new WaitForSeconds(burstGap);
                }
            }
        }

        void Update()
        {
            if (continuous)
            {
                if (Input.GetMouseButton(0) && magazine > 0)
                {
                    laserLine.enabled = true;
                    StartCoroutine(shoot());
                }
                else
                {
                    laserLine.enabled = false;
                }
            }
            else if ((auto ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && fireCountDown <= 0.0f && magazine > 0)
            {
                StartCoroutine(shoot());
                //fireCountDown = 1.0f / fireRate;  <---BAD VERY BAD
                fireCountDown = 60.0f / fireRate; //<---GOOD VERY GOOD
            }
            fireCountDown -= Time.deltaTime;

        }
    }
}
