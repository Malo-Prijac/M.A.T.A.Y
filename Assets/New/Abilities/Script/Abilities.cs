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
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    [SerializeField]private int UnarmedLayer = 0;
    [SerializeField]private int MeleeArmedLayer = 1;
    [SerializeField]private int RangedArmedLayer = 2;

    [ReadOnly][SerializeField]private float layerElapsedTime;
    [SerializeField]private float layerWaitTime = 1f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 600f;
    [SerializeField]private float dashTime = 0.6f;
    [SerializeField]private float dashCoolDown = 1f;
    [ReadOnly][SerializeField]private float currentDashTime;
    [ReadOnly][SerializeField]private float currentDashCooldown;
    [ReadOnly][SerializeField]private bool _isDashing;

    [Header("Jump")]
    [SerializeField]private int numberJumps = 2;
    [ReadOnly][SerializeField]private int countNumberOfJump;
    [ReadOnly][SerializeField]private bool canDash = true;
    
    [Header("Weapons")]
    
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField]private float damageMulti = 1.5f;
    [SerializeField]private GameObject meleeWeapon;
    [ReadOnly][SerializeField]private bool _isAttackingMelee;
    [SerializeField]private string tagAttackMelee = "AttackMelee";
    [ReadOnly][SerializeField]private bool _attackMode;
    [SerializeField]private float _delaySheath = 0.5f;
    [ReadOnly][SerializeField]private bool inTransition;
    [ReadOnly][SerializeField]private bool _isAiming;
    [SerializeField]private GameObject rangedWeapon;
    [SerializeField]private Transform shootPoint;

    [SerializeField]private float aimingDelay = 1f;
    [ReadOnly][SerializeField]private float aimingCounter;
    [ReadOnly] [SerializeField] private bool fullAim;

    public bool FullAim
    {
        get => fullAim;
        set => fullAim = value;
    }
    
    private RangedWeapon _rangedWeaponScript;
    private MeleeWeapon _meleeWeaponScript;

    [Header("Weapon Slots")]
    [SerializeField] private Transform UnarmedMeleeSlot;
    [SerializeField] private Transform ArmedMeleeSlot;
    [SerializeField] private Transform UnarmedRangedSlot;
    [SerializeField] private Transform ArmedRangedSlot;

    [SerializeField] private float rotationSlotSpeed = 10;
    [SerializeField] private float positionSlotSpeed = 10;

    [Header("Input Debug")]
    [ReadOnly][SerializeField] bool _inputAttack;
    [ReadOnly][SerializeField] bool _inputAimUp;
    [ReadOnly][SerializeField] bool _inputAimDown;
    [ReadOnly] [SerializeField] private bool _inputChangeCombat;
    [ReadOnly] [SerializeField] private bool _inputDash;
    [ReadOnly] [SerializeField] private bool _inputJump;
    
    [SerializeField] private bool _hasMeleeWeapon;
    [SerializeField] private bool _hasRangedWeapon;
    private Transform _currentSlot;
    private Rigidbody _rb;
    private PlayerCharacterController _characterController;

    private ThirdPersonShooter _tpsScript;

    public bool HasRangedWeapon
    {
        get => _hasRangedWeapon;
        set => _hasRangedWeapon = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _tpsScript = GetComponent<ThirdPersonShooter>();
        characterAnimator = GetComponent<Animator>();
        canDash = true;
        _rb = GetComponent<Rigidbody>();
        countNumberOfJump = numberJumps;
        _characterController = GetComponent<PlayerCharacterController>();

        _hasMeleeWeapon = false;
        if (meleeWeapon)
        {
            _meleeWeaponScript = meleeWeapon.GetComponent<MeleeWeapon>();
            _meleeWeaponScript.TargetTag = targetTag;
            _hasMeleeWeapon = true;
        }

        _hasRangedWeapon = false;
        if (rangedWeapon)
        {
            _rangedWeaponScript = rangedWeapon.GetComponent<RangedWeapon>();
            _rangedWeaponScript.TargetTag = targetTag;
            _hasRangedWeapon = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        AnimationBehavior();
        bool grounded = _characterController.Grounded;

        if (grounded && !_characterController.Jumping )
        {
            countNumberOfJump = numberJumps;
        }
        //Dash

        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
        }

        if (!inTransition)
        {
            if (_inputDash && canDash && grounded && !_isAttackingMelee)
            {
                Dash();
            }
            
            //Double Jump
            if (_inputJump && countNumberOfJump > 0 && !_isAttackingMelee)
            {
                MultipleJump();
            }

            if (_hasMeleeWeapon)
            {
                if (_inputAttack && !_isDashing && _characterController.Grounded)
                {
                    if(!_attackMode)
                        StartCoroutine(ChangeCombatMode());
                    AttackWithMeleeWeapon();
                }

                if (_inputChangeCombat && _characterController.Grounded)
                {
                    StartCoroutine(ChangeCombatMode());
                }
            }
            

            if (_isAiming)
            {
                if (_inputAttack)
                {
                    _rangedWeaponScript.Attack(_tpsScript.mouseWorldPosition,0,shootPoint.position);
                    //_tpsScript.mouseWorldPosition-_rangedWeaponScript.transform.position
                }
            }


        }

        AimMode();
        ChangeSlot();
        InputPlayer();

    }



    private void AimMode()
    {
        fullAim = Mathf.Approximately(aimingCounter ,aimingDelay);
        if (aimingDelay == 0)
            return;
        if (_isAiming)
        {
            if(aimingCounter<aimingDelay) aimingCounter += Time.deltaTime;
            aimingCounter = aimingCounter >= aimingDelay ? aimingDelay : aimingCounter;
        }
        else
        {
            if(aimingCounter>0) aimingCounter -= Time.deltaTime;
            aimingCounter = aimingCounter <= 0 ? 0 : aimingCounter;
        }

        characterAnimator.SetLayerWeight(RangedArmedLayer,aimingCounter/(aimingDelay));
        
    }


    IEnumerator ChangeLayerMask(int layer, float start, float end, float totalTime)
    {
        layerElapsedTime = 0;
        while (layerElapsedTime < totalTime)
        {
            float weight = Mathf.Lerp(start, end, layerElapsedTime / totalTime);
            characterAnimator.SetLayerWeight(layer,weight);
            
            layerElapsedTime += Time.deltaTime;
            
            yield return null;
        }
        characterAnimator.SetLayerWeight(layer,end);
    }

    void InputPlayer()
    {
        _inputAttack = Input.GetButtonDown("Attack");
        _inputAimDown = Input.GetButtonDown("Aim");
        _inputAimUp = Input.GetButtonUp("Aim");
        _isAiming = Input.GetButton("Aim");
        _inputChangeCombat = Input.GetButtonDown("ChangeCombatMode");
        _inputDash = Input.GetButtonDown("Dash");
        _inputJump = Input.GetButtonDown("Jump");

    }

    IEnumerator ChangeCombatMode()
    {
        if (inTransition || _isDashing || _isAttackingMelee)
            yield break;
        
        inTransition = true;
        _attackMode = !_attackMode;

        if (_attackMode)
            StartCoroutine(ChangeLayerMask(MeleeArmedLayer,0,1,layerWaitTime));
            //characterAnimator.SetLayerWeight(_armedLayer,1);
        
        yield return new WaitForSeconds(_delaySheath);
        
        if(!_attackMode)
            StartCoroutine(ChangeLayerMask(MeleeArmedLayer,1,0,layerWaitTime));
            //characterAnimator.SetLayerWeight(_armedLayer,0);
        inTransition = false;
    }

    private void ChangeSlot()
    {
        if (inTransition && !_attackMode || !_hasMeleeWeapon)
            return;
        AttachWeaponToSlot(_attackMode ? ArmedMeleeSlot : UnarmedMeleeSlot);
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
        _meleeWeaponScript.IsAttacking = true;
        StartCoroutine(StopAttack(tagAttackMelee));

    }
    
    private IEnumerator StopAttack(string tagAnim)
    {
        while (!HelperAnimation.IsAnimationCurrentAnimation(characterAnimator,tagAnim,MeleeArmedLayer))
        {
            yield return null;
        }
        print("ui");
        while (HelperAnimation.AnimatorIsPlaying(characterAnimator,tagAnim,MeleeArmedLayer) )
        {
            yield return null;
        }
        _meleeWeaponScript.IsAttacking = false;
        _isAttackingMelee = false;
    }

    private void MultipleJump()
    {
        countNumberOfJump--;
        _characterController.Jump();
    }

    private void Dash()
    {
        canDash = false;
        //rb.AddForce(transform.forward*dashSpeed,ForceMode.Acceleration);
        //print(rb.velocity);
        StartCoroutine(DashRoutine());
    }

    public void GiveMeleeWeaponToPlayer(GameObject meleeWeaponToGive)
    {
        if (meleeWeaponToGive)
        {
            meleeWeapon = Instantiate(meleeWeaponToGive,UnarmedMeleeSlot.position,UnarmedMeleeSlot.rotation);
            AttachWeaponToSlot(UnarmedMeleeSlot);
            MeleeWeapon scriptWeapon = meleeWeapon.GetComponent<MeleeWeapon>();
            _meleeWeaponScript = scriptWeapon;
            scriptWeapon.TargetTag = targetTag;
            scriptWeapon.Damage *= damageMulti;
            _hasMeleeWeapon = true;
        }
    }
    
    public void GiveRangedWeaponToPlayer(GameObject rangedWeaponToGive)
    {
        if (rangedWeaponToGive)
        {
            meleeWeapon = Instantiate(rangedWeaponToGive,UnarmedRangedSlot.position,UnarmedRangedSlot.rotation);
            AttachWeaponToSlot(UnarmedRangedSlot);
            RangedWeapon scriptWeapon = meleeWeapon.GetComponent<RangedWeapon>();
            _rangedWeaponScript = scriptWeapon;
            scriptWeapon.TargetTag = targetTag;
            scriptWeapon.DamageMulti *= damageMulti;
            _hasRangedWeapon = true;
        }
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }


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
        StartCoroutine(ResetDash());
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
        characterAnimator.SetBool(IsAiming, _isAiming);
        characterAnimator.SetBool(IsAttackingMelee, _isAttackingMelee);
        characterAnimator.SetBool(IsOnAttackMode, _attackMode);

    }
}
