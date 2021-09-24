using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private float safety;
    Animator animator;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Slam");
        safety = 0.0f;
        

    }

    // Update is called once per frame
    void Update()
    {
        safety += Time.deltaTime;
        if(safety > 2.0)
        {
            Destroy(gameObject);
        }
        
    }

    
    

}
