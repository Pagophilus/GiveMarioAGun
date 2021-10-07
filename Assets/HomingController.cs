using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingController : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rigidBody;
    public float rotationSpeed = 0.5f;
    public float forwardSpeed = 4.0f;

    void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        Vector2 direction = ((Vector2)target.position - rigidBody.position).normalized;
        Vector2 forward = new Vector2(Mathf.Cos(rigidBody.rotation * Mathf.PI / 180.0f), Mathf.Sin(rigidBody.rotation * Mathf.PI / 180.0f));
        float rotateAmount = Vector3.Cross(direction, forward).z;
        if (rigidBody.velocity.magnitude > 12.0f)
        {
            rigidBody.AddForce(-1.0f * rigidBody.velocity, ForceMode2D.Force);
        }
        rigidBody.AddTorque(rotateAmount * -10.0f);
        rigidBody.AddForce(forward * 20.0f, ForceMode2D.Force);
    }
}
