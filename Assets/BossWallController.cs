using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Class meant to control the bullet, its movement, and death in a ball of flame.
    /// </summary>
    public class BossWallController : MonoBehaviour
    {
        public GameObject wall;
        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("triggered");
            wall.SetActive(true);
        }

    }
}