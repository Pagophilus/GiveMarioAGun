using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Mechanics.BulletController;
using static Platformer.Mechanics.PlayerController;

public class GunController : MonoBehaviour
{
    public GameObject bullet;

    public int pellets = 5;
    public float spread = 40.0f;
    public float recoil = 8.0f;
    public float fireRate = 4.0f;
    public bool auto = false;
    public bool randomSpread = false;

    private float fireCountDown = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((auto ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && fireCountDown <= 0.0f)
        {
            gameObject.GetComponentInParent<Platformer.Mechanics.PlayerController>().Bounce(transform.up * -1.0f * recoil);
            //gameObject.GetComponentInParent<Rigidbody2D>().AddForce(transform.up * -8.0f, ForceMode2D.Impulse);
            Vector2 fireAngle;
            GameObject o;
            for (int i = 0; i < pellets; i++)
            {
                fireAngle = Quaternion.Euler(0, 0, spread / 2.0f - i * spread / (pellets - 1)) * transform.up;
                o = Instantiate(bullet, transform.position + transform.up * 0.5f, Quaternion.identity);
                o.GetComponent<Rigidbody2D>().AddForce(fireAngle * 20.0f, ForceMode2D.Impulse);
            }

            fireCountDown = 1.0f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }
}
