using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int maxHP = 1;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => currentHP > 0;

        private TimerController timer;

        int currentHP;

        private bool dead = false;

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            Damage(1);
        }

        public void Damage(int damage)
        {
            int oldHP = currentHP;
            currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
            Debug.Log("ddd1 decrementing " + currentHP);
            var enemy = gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.OnDamaged(currentHP, oldHP);
            }
            if (currentHP == 0 && !dead)
            {
                dead = true;
                if (enemy != null)
                {
                    timer.Damage(-10);
                    Schedule<EnemyDeath>().enemy = enemy;
                }
                else if (gameObject.GetComponent<PlayerController>())
                {
                    timer.Damage(10);
                    //var ev = Schedule<HealthIsZero>();
                    //ev.health = this;
                } else
                {
                    Destroy(gameObject);
                }

            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            while (currentHP > 0) Decrement();
        }

        void Awake()
        {
            currentHP = maxHP;
            timer = GameObject.Find("Timer").GetComponent<TimerController>();
        }
    }
}
