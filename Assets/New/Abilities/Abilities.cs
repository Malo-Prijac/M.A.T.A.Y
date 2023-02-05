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
    [Header("Animation")]
    [ReadOnly][SerializeField] private Animator characterAnimator;
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    private static readonly int IsAttackingMelee = Animator.StringToHash("IsAttackingMelee");
    private static readonly int IsOnAttackMode = Animator.StringToHash("IsOnAttackMode");

    [SerializeField]private int _unarmedLayer = 0;
    [SerializeField]private int _armedLayer = 1;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField]private float dashTime = 0.5f;
    [SerializeField]private float dashCoolDown = 1f;
    public AudioSource dashSound;

    [Header("Jump")]
    //[SerializeField]private float jumpForce = 5f;
    [SerializeField]private int additionalJump = 2;
    [ReadOnly][SerializeField]private int countAdditionalJump;
    [ReadOnly][SerializeField]private bool canDash = true;
    
    [Header("Dash")]
    [ReadOnly][SerializeField]private float currentDashTime;
    [ReadOnly][SerializeField]private float currentDashCooldown;
    [ReadOnly][SerializeField]private bool _isDashing;
    public float bulletSpeed = 100f;


    private PlayerCharacterController _characterController;
    
    [Header("Weapons")]
    
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField]private float damageMulti = 1.5f;
    [ReadOnly][SerializeField]private GameObject meleeWeapon;
    [ReadOnly][SerializeField]private bool _isAttackingMelee;
    [SerializeField]private string tagAttackMelee = "AttackMelee";
    [ReadOnly][SerializeField]private bool _attackMode;
    [SerializeField]private float _delaySheath = 0.5f;
    [ReadOnly][SerializeField]private bool inTransition;

    [Header("Weapon Slots")]
    [SerializeField] 
    private Transform weaponSlotUnarmed;
    [SerializeField] 
    private Transform weaponSlotArmed;
    [SerializeField] 
    private float rotationSlotSpeed = 10;
    [SerializeField] 
    private float positionSlotSpeed = 10;

    private bool _hasMeleeWeapon;
    private Transform _currentSlot;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        
        characterAnimator = GetComponent<Animator>();
        canDash = true;
        _rb = GetComponent<Rigidbody>();
        countAdditionalJump = additionalJump;
        _characterController = GetComponent<PlayerCharacterController>();

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

        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
        }

        if (!inTransition)
        {
            if (Input.GetButtonDown("Dash") && canDash && grounded && !_isAttackingMelee)
            {
                Dash();
                //StartCoroutine(DashRoutine());
            }
            
            //Double Jump
            if (Input.GetButtonDown("Jump") && countAdditionalJump > 0 && !_isAttackingMelee)
            {
                if (_attackMode)
                    StartCoroutine(ChangeCombatMode());
                MultipleJump();
            }

            if (_hasMeleeWeapon)
            {
                if (Input.GetButtonDown("AttackMelee") && !_isDashing && _characterController.Grounded)
                {
                    if(!_attackMode)
                        StartCoroutine(ChangeCombatMode());
                    AttackWithMeleeWeapon();
                }

                if (Input.GetButtonDown("ChangeCombatMode") && _characterController.Grounded)
                {
                    StartCoroutine(ChangeCombatMode());
                }
            }

            
        }

        
        ChangeSlot();
    }

    IEnumerator ChangeCombatMode()
    {
        if (inTransition || _isDashing || _isAttackingMelee)
            yield break;
        
        print(_isDashing);
        inTransition = true;
        _attackMode = !_attackMode;
        
        if (_attackMode)
            characterAnimator.SetLayerWeight(_armedLayer,1);
        
        yield return new WaitForSeconds(_delaySheath);
        
        if(!_attackMode)
            characterAnimator.SetLayerWeight(_armedLayer,0);
        inTransition = false;
    }

    private void ChangeSlot()
    {
        if (inTransition && !_attackMode || !_hasMeleeWeapon)
            return;
        AttachWeaponToSlot(_attackMode ? weaponSlotArmed : weaponSlotUnarmed);
    }
    

    private void AttachWeaponToSlot(Transform slot)
    {
        _currentSlot = slot;
        meleeWeapon.transform.parent = _currentSlot;
        Vector3 interpolatedPosition = Vector3.Lerp(meleeWeapon.transform.position, _currentSlot.position,Time.deltaTime * positionSlotSpeed);

        Quaternion interpolatedAngle = Quaternion.Slerp (meleeWeapon.transform.rotation, _currentSlot.rotation, Time.deltaTime * rotationSlotSpeed);

        meleeWeapon.transform.position = interpolatedPosition;
        meleeWeapon.transform.rotation = interpolatedAngle;
    }

    private void AttackWithMeleeWeapon()
    {
        _isAttackingMelee = true;
        StartCoroutine(StopAttack(tagAttackMelee));

    }
    
    private IEnumerator StopAttack(string tagAnim)
    {
        while (!HelperAnimation.IsAnimationCurrentAnimation(characterAnimator,tagAnim,_armedLayer))
        {
            yield return null;
        }
        print("ui");
        while (HelperAnimation.AnimatorIsPlaying(characterAnimator,tagAnim,_armedLayer) )
        {
            yield return null;
        }
        _isAttackingMelee = false;
    }

    private void MultipleJump()
    {
        countAdditionalJump--;
        _characterController.Jump();
    }

    private void Dash()
    {
        canDash = false;
        //rb.AddForce(transform.forward*dashSpeed,ForceMode.Acceleration);
        //print(rb.velocity);
        StartCoroutine(DashRoutine());
        StartCoroutine(ResetDash());
    }

    public void GiveMeleeWeaponToPlayer(GameObject meleeWeaponToGive)
    {
        if (meleeWeaponToGive)
        {
            meleeWeapon = Instantiate(meleeWeaponToGive,weaponSlotUnarmed.position,weaponSlotUnarmed.rotation);
            AttachWeaponToSlot(weaponSlotUnarmed);
            MeleeWeapon scriptWeapon = meleeWeapon.GetComponent<MeleeWeapon>();
            scriptWeapon.TargetTag = targetTag;
            scriptWeapon.Damage *= damageMulti;
            _hasMeleeWeapon = true;
        }
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
        
        characterAnimator.SetBool(IsAttackingMelee, _isAttackingMelee);
        characterAnimator.SetBool(IsOnAttackMode, _attackMode);

    }
}
