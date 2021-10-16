using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissileController : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rigidBody;
    public GameObject explosion;

    void Awake()
    {
        target = GameObject.Find("Donut").transform;
    }

    void FixedUpdate()
    {
        Debug.Log("ddd " + target.position);
        Vector2 newDir = (target.position - transform.position).normalized;
        transform.right = newDir;// target.position - transform.position;
        rigidBody.velocity = newDir * 10.0f;
        if (((Vector2) (target.position - transform.position)).magnitude < 0.2f)
        {
            if (explosion != null)
            {
                Instantiate(explosion, (Vector2)transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
