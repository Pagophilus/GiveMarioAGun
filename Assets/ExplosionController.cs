using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class ExplosionController : MonoBehaviour
    {
        private float safety;
        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log("EXPLOSION MADE");
            animator = GetComponent<Animator>();
            animator.Play("RocketTpact1");
            safety = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            safety += Time.deltaTime;
            if (safety > 2.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}