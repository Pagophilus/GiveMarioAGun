using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class TimerController : MonoBehaviour
    {
        public float timeLeft = 100.0f;
        public GameObject player;
        private HUDController hud;

        // Start is called before the first frame update
        void Start()
        {
            hud = GameObject.Find("Hud").GetComponent<HUDController>();
        }

        // Update is called once per frame
        void Update()
        {
            Damage(Time.deltaTime);
            if (timeLeft <= 0)
            {
                player.GetComponent<PlayerController>().Die();
                timeLeft = 100.0f;
            }
            hud.UpdateTimer(timeLeft);
        }

        public void Damage(float damage)
        {
            timeLeft -= damage;
        }
    }
}
