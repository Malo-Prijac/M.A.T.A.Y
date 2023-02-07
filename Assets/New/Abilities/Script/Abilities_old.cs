using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/*
 Aim and shoot at the reticule
 Cooldown UI for Dash & Shoot  
*/

public class Abilities_old : MonoBehaviour
{
    /*
    [SerializeField] private Animator charaterAnimator;
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    */
    [Header("Animation")]
    [ReadOnly][SerializeField] private Animator characterAnimator;
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");

    [Header("Dash")] 
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 5f;
    [SerializeField] private float dashCoolDown = 1f;
    public AudioSource dashSound;
    [SerializeField] private Image dashImage;

    [Header("Jump")]
    //[SerializeField]private float jumpForce = 5f;
    [SerializeField]private int additionalJump = 2;
    [ReadOnly][SerializeField]private int countAdditionalJump;
    [ReadOnly][SerializeField]private bool canDash = true;
    public AudioSource jumpSound;
    
    [ReadOnly][SerializeField]private float currentDashTime;
    [ReadOnly][SerializeField]private float currentDashCooldown;
    [ReadOnly][SerializeField]private bool _isDashing;
    public float bulletSpeed = 100f;

    [Header("Shoot")] 
    private Rigidbody _rb;
    public GameObject bulletPrefab;
    public Transform targetCamera;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject bulletSpawn;
    public AudioSource shootSound;

    private PlayerCharacterController _characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = GetComponent<Animator>();
        canDash = true;
        _rb = GetComponent<Rigidbody>();
        countAdditionalJump = additionalJump;
        _characterController = GetComponent<PlayerCharacterController>();
        dashImage.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationBehavior();
        bool grounded = _characterController.Grounded;

        if (grounded)
        {
            countAdditionalJump = additionalJump;
        }
        //Dash
        if (Input.GetButtonDown("Dash") && canDash && grounded)
        {
            Debug.Log("Dashing");
            Dash();
            //StartCoroutine(DashRoutine());
        }

        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= 1 / dashCoolDown * Time.deltaTime;
            dashImage.fillAmount = currentDashCooldown;
            if (dashImage.fillAmount <= 0)
            {
                dashImage.fillAmount = 0;
            }
        }
        else
        {
            dashImage.fillAmount = 1;
        }
        
        //Double Jump
        if (Input.GetButtonDown("Jump") && countAdditionalJump > 0)
        {
            MultipleJump();
        }
        
        //Shoot
        if (Input.GetMouseButtonDown(0))
        {   
            shootSound.Play();
            Shoot();
        }
        
        //Aiming
        if (Input.GetMouseButton(1))
        {
            crossHair.SetActive(true);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    private void MultipleJump()
    {
        countAdditionalJump--;
        _characterController.Jump();
    }

    void Shoot()
    {
        Debug.Log("Shooting");
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(targetCamera.forward * bulletSpeed, ForceMode.VelocityChange);
        Destroy(bullet,1);
    }

    private void Dash()
    {
        canDash = false;
        //rb.AddForce(transform.forward*dashSpeed,ForceMode.Acceleration);
        //print(rb.velocity);
        StartCoroutine(DashRoutine());
        StartCoroutine(ResetDash());
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCoolDown);

        canDash = true;
    }
    /*
    IEnumerator ResetDash()
    {
        yield return dashCoolDown;

        isDashing = false;
    }*/

    
    IEnumerator DashRoutine()
    {
        _isDashing = true;
        currentDashTime = dashTime;
        //dashSound.Play();
        Vector3 start = transform.position;
        while (currentDashTime > 0)
        {
            //rb.velocity = transform.forward * dashSpeed;
            //rb.velocity = transform.forward * dashSpeed;
            //rb.AddForce(transform.forward*dashSpeed,ForceMode.VelocityChange);
            _rb.drag = 0;
            _rb.velocity = new Vector3();
            _rb.AddForce(dashSpeed*Time.deltaTime*transform.forward,ForceMode.VelocityChange);
            currentDashTime -= Time.deltaTime;
            yield return null;
        }
        //print((start - transform.position).magnitude);

        _isDashing = false;
        currentDashCooldown = dashCoolDown;
    }
    
    private void AnimationBehavior()
    {
        if (!characterAnimator)
        {
            Debug.LogWarning("No Animator Character on "+name);
            return;  
        }
        
        //characterAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
        
        //characterAnimator.SetFloat(VelocityHash,velocity);
        
        characterAnimator.SetBool(IsDashing, _isDashing);
    }
}
