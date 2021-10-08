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
        public GameObject newHaven;
        protected bool dead = false;
        public GameObject[] pillars;

        public string bossName = "The Boss";
        public int maxHP = 100;

        private HUDController hud;

        public override void OnDamaged(int newHP, int oldHP)
        {
            hud.UpdateBossHP(newHP, maxHP);

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
            hud.UpdateBossHP(maxHP, maxHP);
        }

        void OnDisable()
        {
            hud.UpdateBossName("");
        }
    }
}