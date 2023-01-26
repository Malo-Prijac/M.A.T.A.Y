using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    /*
    [SerializeField] private Animator charaterAnimator;
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    */
    
    public float dashSpeed = 1f;
    public float dashTime = 0.5f;
    public float dashCoolDown = 1f;
    public float jumpForce = 5f;
    public int jumpCount = 2;
    private int currentJumpCount;
    public AudioSource dashSound;
    public AudioSource jumpSound;
    private float currentDashTime;
    private float currentDashCooldown;
    private bool isDashing;
    public float bulletSpeed = 100f;

    private Rigidbody rb;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public Transform targetCamera;
    
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
        if (Input.GetKeyDown(KeyCode.C) == true && !isDashing && currentDashCooldown <= 0)
        {
            Debug.Log("Dashing");
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
        if (jumpSound)
        {
            jumpSound.Play();
        }
        else
        {
            Debug.LogWarning("no sound for jump");
        }
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
        GameObject bullet = Instantiate(bulletPrefab, targetCamera.position, targetCamera.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);
        Destroy(bullet,1);
    }
    
    IEnumerator DashRoutine()
    {
        isDashing = true;
        currentDashTime = dashTime;
        //dashSound.Play();
        while (currentDashTime > 0)
        {
            //rb.velocity = transform.forward * dashSpeed;
            rb.AddForce(transform.forward*dashSpeed,ForceMode.VelocityChange);
            currentDashTime -= Time.deltaTime;
            yield return null;
        }

        isDashing = false;
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
