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

    void Update()
    {

    /*    Vector2 direction = (Vector2)target.position - rigidBody.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
        rigidBody.velocity = transform.up * movementSpeed;
        Debug.Log("ddd 1 target " + rigidBody.velocity);*/

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("ddd 1 target " + angle);

  //       angle = Mathf.Clamp(angle, -Time.deltaTime * rotationSpeed, Time.deltaTime * rotationSpeed);
        Debug.Log("ddd 2 target " + angle);

        var rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, 0.0000000000000000000001f);//Time.deltaTime * rotationSpeed);
        rigidBody.velocity = new Vector2(direction.x * forwardSpeed, direction.y * forwardSpeed);

    }
}
