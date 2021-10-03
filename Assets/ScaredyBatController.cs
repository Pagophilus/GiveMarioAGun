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
    public class ScaredyBatController : EnemyController
    {
        protected override void Update()
        {
            float away = (transform.position.x > thePlayer.transform.position.x ? 3.0f : -3.0f);
            if (Vector2.Distance(thePlayer.transform.position, transform.position) < 6.0f)
            {
                if (gameObject.GetComponent<Rigidbody2D>().velocity.x < 10.0f) {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(away, 0.0f), ForceMode2D.Force);
                } 
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-away, 0.0f), ForceMode2D.Force);
            }
            base.Update();
        }
    }
}