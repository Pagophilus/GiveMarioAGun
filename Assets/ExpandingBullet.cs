using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Mechanics
{
    public class ExpandingBullet : MonoBehaviour
    {
        public float duration;
        private float startTime;
        private Vector3 finalScale;
        // Start is called before the first frame update
        void Start()
        {
            startTime = Time.time;
            finalScale = transform.localScale;


        }

        // Update is called once per frame
        void Update()
        {
            float d = (Time.time - startTime) / duration;
            if (d > 1.0f)
            {
                transform.localScale = finalScale;
            } else
            {
                transform.localScale = d * finalScale;

            }
        }
    }
}
