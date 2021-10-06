using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Platformer.Mechanics.BulletController;
using static Platformer.Mechanics.PlayerController;
using static Platformer.Mechanics.EnemyController;
using static Platformer.Gameplay.EnemyDeath;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class HavenController : MonoBehaviour
    {
        public TimerController timer;

        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("ddd1 entering");
            timer.pause();
            GameObject.Find("GameController").GetComponent<GameController>().model.spawnPoint = transform;
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("ddd12 exiting");
            timer.unpause();
        }
    }
}
