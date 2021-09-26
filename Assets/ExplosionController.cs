using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float safety;
    Animator animator;
    int counter;
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
        if (safety > 2.0)
        {
            Destroy(gameObject);
        }

    }




}
