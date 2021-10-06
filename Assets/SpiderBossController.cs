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
    public class SpiderBossController : EnemyController
    {
        public GameObject barrier;
        public GameObject egg;
        public GameObject newHaven;
        private int phase = 1;
        public override void OnDamaged(int newHP, int oldHP)
        {
            Debug.Log("onDamaged " + (oldHP / 20) + " " + (newHP / 20));
            if (oldHP / 20 != newHP / 20)
            {
                for (int i = -2; i < 2; i++)
                {
                    Vector2 fireAngle = new Vector2(i, 1).normalized;
                    //fireAngle.Normalize();
                    GameObject o = Instantiate(egg, transform.position, Quaternion.identity);
                    o.GetComponent<Rigidbody2D>().AddForce(fireAngle * launchSpeed, ForceMode2D.Impulse);
                    //barrier.SetActive(true);
                    phase = 2;
                }
                //Instantiate(this, new Vector3(transform.position.x + 2 * (thePlayer.transform.position.x - transform.position.x), transform.position.y, transform.position.z), Quaternion.identity);
            }
            if (newHP <= 0 && oldHP > 0)
            {
                newHaven.SetActive(true);
            }
        }

        protected override void Update()
        {
            if (fireDelay < 0.0f && Vector2.Distance(thePlayer.transform.position, transform.position) < 6.0f)
            {
                float rand = Random.value;
                if (rand < 0.2f)
                {
                    if (control.enabled)
                    {
                        control.enabled = false;
                    } else
                    {
                        control.enabled = true;
                    }
                    fireDelay = 2.0f;
                }
                else
                {
                    Vector2 fireAngle = thePlayer.transform.position - transform.position;
                    fireAngle.Normalize();
                    GameObject o = Instantiate(bullet, transform.position, Quaternion.identity);
                    o.GetComponent<Rigidbody2D>().AddForce(fireAngle * launchSpeed, ForceMode2D.Impulse);
                    for (int i = 1; i <= phase; i++)
                    {
                        o = Instantiate(bullet, transform.position, Quaternion.identity);
                        o.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, 15.0f * i) * fireAngle * launchSpeed, ForceMode2D.Impulse);
                        o = Instantiate(bullet, transform.position, Quaternion.identity);
                        o.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, -15.0f * i) * fireAngle * launchSpeed, ForceMode2D.Impulse);
                    }
                    fireDelay = 2.0f;
                }
            }
            fireDelay -= Time.deltaTime;
            if (path != null && control.enabled)
            {
                if (mover == null)
                {
                    mover = path.CreateMover(control.maxSpeed * 0.5f);
                }
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
                control.move.y = Mathf.Clamp(mover.Position.y - transform.position.y, -1, 1);
            }
        }
    }
}