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
        private bool timerRunning = true;
        private bool godMode = false;

        // Start is called before the first frame update
        void Start()
        {
            hud = GameObject.Find("Hud").GetComponent<HUDController>();
        }

        public void pause()
        {
            timerRunning = false;
        }

        public void unpause()
        {
            timerRunning = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("f"))
            {
                godMode = !godMode;
            }
            
            if (timerRunning && !godMode)
            {
                Damage(Time.deltaTime);
                if (timeLeft <= 0)
                {
                    player.GetComponent<PlayerController>().Die();
                    timeLeft = 100.0f;
                }
                hud.UpdateTimer(timeLeft);
            }
        }

        public void Damage(float damage)
        {
            if (!godMode)
            {
                timeLeft -= damage;
            }
        }
    }
}
