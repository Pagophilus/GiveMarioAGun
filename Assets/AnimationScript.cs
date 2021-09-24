using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    Animator animator;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Slam");
        

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    
    

}
