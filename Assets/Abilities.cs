using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public float dashSpeed = 5f;
    public float dashTime = 0.5f;
    public float dashCoolDown = 1f;
    public float jumpForce = 5f;
    public int jumpCount = 2;
    private int currentJumpCount;
    public AudioSource dashSound;
    public AudioSource jumpSound;
    public TrailRenderer trail;
    private float currentDashTime;
    private float currentDashCooldown;
    private bool isDashing;
    private Rigidbody rb;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentJumpCount = jumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        //Dash
        if (Input.GetKeyDown(KeyCode.D) == true && !isDashing && currentDashCooldown <= 0)
        {
            StartCoroutine(DashRoutine());
        }

        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
        }
        
        //Double Jump
        if (Input.GetKeyDown(KeyCode.F) && currentJumpCount > 0)
        { 
            DoubleJump();
        }
        
        //Shoot
        if (Input.GetKeyDown(KeyCode.B))
        {   
            Shoot();
        }
    }

    void DoubleJump()
    {
        Debug.Log(currentJumpCount);
        Debug.Log("Jumping");
        jumpSound.Play();
        if (currentJumpCount == 1)
        {
            jumpForce = 6;
        }
        //rb.velocity = Vector2.up * jumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        currentJumpCount--;
    }

    void Shoot()
    {
        Debug.Log("Shooting");
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * bulletSpeed;
        Destroy(bullet,2);
    }
    
    IEnumerator DashRoutine()
    {
        isDashing = true;
        currentDashTime = dashTime;
        dashSound.Play();
        trail.emitting = true;
        
        while (currentDashTime > 0)
        {
            //rb.velocity = transform.forward * dashSpeed;
            rb.AddForce(transform.forward*dashSpeed,ForceMode.Impulse);
            currentDashTime -= Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        trail.emitting = false;
        currentDashCooldown = dashCoolDown;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            currentJumpCount = jumpCount;
        }
    }
}
