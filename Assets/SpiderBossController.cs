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
                for (int i = -3; i <= 3; i+=2)
                {
                    Vector2 fireAngle = new Vector2(i, 1).normalized;
                    GameObject o = Instantiate(egg, transform.position, Quaternion.identity);
                    o.GetComponent<Rigidbody2D>().AddForce(fireAngle * launchSpeed, ForceMode2D.Impulse);
                    phase = 2;
                }
            }
            if (newHP <= 0 && oldHP > 0)
            {
                newHaven.SetActive(true);
                    gameObject.SetActive(false);
            }
        }

        protected override void Update()
        {
            if (fireDelay < 0.0f && Vector2.Distance(thePlayer.transform.position, transform.position) < 12.0f)
            {
                float rand = Random.value;
                if (rand < 0.2f)
                {
                    Debug.Log("jumping");
                    GetComponent<Rigidbody2D>().AddForce(new Vector2((thePlayer.transform.position.x - transform.position.x) * 0.4f, 10.0f) * 1.0f, ForceMode2D.Impulse);
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
            /*if (path != null)
            {
                if (mover == null)
                {
                    mover = path.CreateMover(4.0f);
                }
                GetComponent<Rigidbody2D>().velocity = ((Vector3)mover.Position - transform.position).normalized * 10.0f;
            }*/
        }
    }
}