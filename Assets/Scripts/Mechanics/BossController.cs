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
    public class BossController : EnemyController
    {
        public GameObject barrier;
        public GameObject delayedExplosion;
        public GameObject newHaven;
        public GameObject beam;
        private int phase = 1;
        private bool beamOn = false;
        private bool dead = false;
        public GameObject[] pillars;

        private HUDController hud;

        public override void OnDamaged(int newHP, int oldHP)
        {
            hud.UpdateBossHP(newHP, 100);

            if (newHP < 50 && oldHP >= 50)
            {
                barrier.SetActive(true);
                StartCoroutine(ActivateBeam());
                phase = 2;
                //Instantiate(this, new Vector3(transform.position.x + 2 * (thePlayer.transform.position.x - transform.position.x), transform.position.y, transform.position.z), Quaternion.identity);
            }
            if (newHP <= 0 && oldHP > 0)
            {
                newHaven.SetActive(true);
                dead = true;
                foreach (GameObject pillar in pillars)
                {
                    pillar.SetActive(false);
                }
                gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            hud = GameObject.Find("Hud").GetComponent<HUDController>();
            hud.UpdateBossName("The Boss");
            hud.UpdateBossHP(100, 100);
        }

        void OnDisable()
        {
            hud.UpdateBossName("");
        }

        private IEnumerator ActivateBeam()
        {
            beam.SetActive(true);
            beamOn = true;
            yield return new WaitForSeconds(6.0f);
            beam.SetActive(false);
            beamOn = false;
        }
        private IEnumerator StartSpawn()
        {
            for (int i = 0; i < 8 && !dead; i++)
            {
                if (i % 2 == 1 && phase == 2)
                {
                    Instantiate(delayedExplosion, thePlayer.transform.position + new Vector3(1, 1, 0), Quaternion.identity);
                    Instantiate(delayedExplosion, thePlayer.transform.position + new Vector3(-1, -1, 0), Quaternion.identity);
                    Instantiate(delayedExplosion, thePlayer.transform.position + new Vector3(1, -1, 0), Quaternion.identity);
                    Instantiate(delayedExplosion, thePlayer.transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(delayedExplosion, thePlayer.transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(.5f);
            }
        }

        protected override void Update()
        {
            if (beamOn)
            {
                beam.transform.Rotate(Vector3.forward * (120.0f * Time.deltaTime));
            }
            if (fireDelay < 0.0f && Vector2.Distance(thePlayer.transform.position, transform.position) < 15.0f)
            {
                if (Random.value < -0.2f)
                {
                    Vector3 pull = transform.position - thePlayer.transform.position;
                    pull.Normalize();
                    thePlayer.GetComponent<PlayerController>().Bounce(pull * 3.0f);
                    //control.Teleport(new Vector3(transform.position.x + 2 * (thePlayer.transform.position.x - transform.position.x), transform.position.y, transform.position.z));
                    fireDelay = 1.0f;
                } else if (Random.value < 0.4f)
                {
                    StartCoroutine(StartSpawn());
                    fireDelay = 4.0f;
                }
                else
                {
                    Vector2 fireAngle = thePlayer.transform.position - transform.position;
                    fireAngle.Normalize();
                    GameObject o = Instantiate(bullet, transform.position, Quaternion.identity);
                    o.GetComponent<Rigidbody2D>().AddForce(fireAngle * launchSpeed, ForceMode2D.Impulse);
                    o.transform.right = fireAngle;
                    for (int i = 1; i <= phase; i++)
                    {
                        o = Instantiate(bullet, transform.position, Quaternion.identity);
                        o.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, 15.0f * i) * fireAngle * launchSpeed, ForceMode2D.Impulse);
                        o.transform.right = fireAngle;
                        o = Instantiate(bullet, transform.position, Quaternion.identity);
                        o.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, -15.0f * i) * fireAngle * launchSpeed, ForceMode2D.Impulse);
                        o.transform.right = fireAngle;

                    }
                    fireDelay = 2.0f;
                }
            }
            fireDelay -= Time.deltaTime;
            if (path != null)
            {
                if (mover == null)
                {
                    mover = path.CreateMover(4.0f);
                }
                GetComponent<Rigidbody2D>().velocity = ((Vector3)mover.Position - transform.position).normalized * 10.0f;
            }
        }
    }
}