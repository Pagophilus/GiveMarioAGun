using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Platformer.Mechanics.BulletController;
using static Platformer.Mechanics.PlayerController;
using static Platformer.Mechanics.EnemyController;
using static Platformer.Gameplay.EnemyDeath;
using  Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{

    public class LymphController : GunController
    {
        public GameObject lockOn;
        [SerializeField] private LayerMask enemyLayer;
        public override IEnumerator shoot()
        {
            foreach (Collider2D enemy in Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f, 6.666f * Mathf.Min(1.0f, chargeTime / minCharge), enemyLayer))
            {
                var fireAngle = ((Vector2) enemy.transform.position - (Vector2) transform.position).normalized;
                // Check if the target is in a 60 degree wedge.
                if (magazine > 0 && Vector2.Dot(transform.up, fireAngle) > 0.86602540378f)
                {
                    magazine--;
                    UpdateAmmo();
            
                    Instantiate(lockOn, enemy.transform.position, Quaternion.identity);
                    transform.parent.GetComponent<Rigidbody2D>().AddForce(transform.up * -1.0f * recoil, ForceMode2D.Impulse);
                    GameObject o = Instantiate(bullet, transform.position + transform.up * 0.5f, Quaternion.identity);
                    o.GetComponent<Rigidbody2D>().AddForce(fireAngle * (bulletForce + bulletForceVariance * Random.value), ForceMode2D.Impulse);
                }
            }
            yield return new WaitForSeconds(0.0f);
        }
    }   
}
