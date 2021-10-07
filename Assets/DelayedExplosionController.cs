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
    public class DelayedExplosionController : MonoBehaviour
    {
        public GameObject explosion;

        // Start is called before the first frame update
        void Awake()
        {
            StartCoroutine(Spawn());
            Destroy(gameObject, 1.0f);

        }

        IEnumerator Spawn()
        {
            yield return new WaitForSeconds(0.8f);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
