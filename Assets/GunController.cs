using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Mechanics.BulletController;
using static Platformer.Mechanics.PlayerController;
using static Platformer.Mechanics.EnemyController;
using static Platformer.Gameplay.EnemyDeath;
using  Platformer.Gameplay;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject impactFX;

    public int pellets = 5;
    public float spread = 40.0f;
    public float recoil = 8.0f;
    public float fireRate = 4.0f;
    public bool auto = false;
    public bool hitscan = false;
    public bool continuous = false;
    public bool randomSpread = false;
    private LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline

    private float fireCountDown = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        if (laserLine != null)
        {
            laserLine.sortingLayerName = "Foreground";
        }
    }


    void shoot()
    {
        gameObject.GetComponentInParent<Rigidbody2D>().AddForce(transform.up * -1.0f * recoil, ForceMode2D.Impulse);

        if (hitscan)
        {
            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit2D hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            if (laserLine != null)
            {
                laserLine.SetPosition(0, transform.position + transform.up * 0.5f);
            }

            Debug.Log("getting hits");
            // Check if our raycast has hit anything
            hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, 100.0f);
            if (hit.collider != null)
            {
                //Debug.Log("hit found: " + hit.point.x + " " + hit.point.y + " " + hit.collider.gameObject.name);

                if (laserLine != null)
                {
                    // Set the end position for our laser line 
                    laserLine.SetPosition(1, hit.point);
                }

                Instantiate(impactFX, hit.point + (Vector2)transform.up * -0.02f, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Debug.Log("no hit found: ");

                if (laserLine != null)
                {
                    // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                    laserLine.SetPosition(1, transform.position + transform.up * 50.0f);
                }
            }
        } else
        {
            //gameObject.GetComponentInParent<Platformer.Mechanics.PlayerController>().Bounce(transform.up * -1.0f * recoil);
            //gameObject.GetComponentInParent<Rigidbody2D>().AddForce(transform.up * -1.0f * recoil, ForceMode2D.Impulse);
            Vector2 fireAngle;
            GameObject o;
            if (pellets != 1)
            {
                for (int i = 0; i < pellets; i++)
                {
                    fireAngle = Quaternion.Euler(0, 0, spread / 2.0f - i * spread / (pellets - 1)) * transform.up;
                    o = Instantiate(bullet, transform.position + transform.up * 0.5f, Quaternion.identity);
                    o.GetComponent<Rigidbody2D>().AddForce(fireAngle * 20.0f, ForceMode2D.Impulse);
                }
            }
            else
            {
                fireAngle = transform.up;
                o = Instantiate(bullet, transform.position + transform.up * 0.5f, Quaternion.identity);
                o.GetComponent<Rigidbody2D>().AddForce(fireAngle * 20.0f, ForceMode2D.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (continuous)
        {
            if (Input.GetMouseButton(0))
            {
                laserLine.enabled = true;
                shoot();
            } else
            {
                laserLine.enabled = false;
            }
        }
        else if ((auto ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && fireCountDown <= 0.0f)
        {
            shoot();
            fireCountDown = 1.0f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }
}
