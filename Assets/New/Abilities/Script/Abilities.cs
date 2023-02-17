using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]private Image spriteDash;

    [Header("Jump")]
    [SerializeField] private int numberJumps = 1;
    [ReadOnly][SerializeField] private int countNumberOfJump;
    [ReadOnly] [SerializeField] private bool canDash;
    [ReadOnly] [SerializeField] private bool hasDash;

    [Header("Weapons")]
    
    [SerializeField]private float delayShoot = 0.8f;
    [ReadOnly][SerializeField]private float counterShoot;

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

    [SerializeField]private float aimingDelay = 1f;
    [ReadOnly][SerializeField]private float aimingCounter;
    [ReadOnly] [SerializeField] private bool fullAim;

    [SerializeField]private float attackModeDelay = 1f;
    [ReadOnly][SerializeField]private float attackModeCounter;
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
    
    [SerializeField] public bool _hasMeleeWeapon;
    [SerializeField] public bool _hasRangedWeapon;
    private Transform _currentSlotRanged;
    private Transform _currentSlotMelee;

    private Rigidbody _rb;
    private PlayerCharacterController _characterController;

    private ThirdPersonShooter _tpsScript;


    public void GetObjectives()
    {
        hasDash = PlayerPrefs.GetInt("hasDash",0) == 1;
        _hasMeleeWeapon = PlayerPrefs.GetInt("_hasMeleeWeapon",0) == 1;
        _hasRangedWeapon = PlayerPrefs.GetInt("_hasRangedWeapon",0) == 1;
        numberJumps = PlayerPrefs.GetInt("numberJumps",1);
        spriteDash.enabled = hasDash;
    }
    public void SetObjectives()
    {
        PlayerPrefs.SetInt("hasDash",hasDash ? 1 : 0);
        PlayerPrefs.SetInt("numberJumps",numberJumps);
        PlayerPrefs.SetInt("_hasMeleeWeapon",_hasMeleeWeapon ? 1 : 0);
        PlayerPrefs.SetInt("_hasRangedWeapon",_hasRangedWeapon ? 1 : 0);
    }
    
    public bool HasRangedWeapon
    {
        get => _hasRangedWeapon;
        set => _hasRangedWeapon = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteDash.enabled = false;
        _tpsScript = GetComponent<ThirdPersonShooter>();
        characterAnimator = GetComponent<Animator>();
        canDash = true;
        _rb = GetComponent<Rigidbody>();
        countNumberOfJump = numberJumps;
        _characterController = GetComponent<PlayerCharacterController>();

        if (meleeWeapon)
        {
            _meleeWeaponScript = meleeWeapon.GetComponent<MeleeWeapon>();
            _meleeWeaponScript.TargetTag = targetTag;
        }

        if (rangedWeapon)
        {
            _rangedWeaponScript = rangedWeapon.GetComponent<RangedWeapon>();
            _rangedWeaponScript.TargetTag = targetTag;
        }

        GetObjectives();
    }

    // Update is called once per frame
    void Update()
    {
        if (meleeWeapon)
            meleeWeapon.SetActive(_hasMeleeWeapon);
        if (rangedWeapon)
            rangedWeapon.SetActive(_hasRangedWeapon);

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
            if (_inputDash && canDash && grounded && !_isAttackingMelee && hasDash)
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
                if (_inputAttack && !_isDashing && _characterController.Grounded && !_isAiming)
                {
                    if (!_attackMode)
                        _attackMode = true;
                    AttackWithMeleeWeapon();
                }
                
                if (_inputChangeCombat && _characterController.Grounded && !inTransition)
                {
                    _attackMode = !_attackMode;
                }
            }
            

            if (_isAiming)
            {
                if (_attackMode)
                    _attackMode = false;

                if (_inputAttack && Mathf.Approximately(counterShoot,delayShoot))
                {
                    counterShoot = 0;
                    _rangedWeaponScript.Attack(_tpsScript.mouseWorldPosition,0,_rangedWeaponScript.transform.position);
                }
            }
        }
        
        
        InputPlayer();

        if (_hasMeleeWeapon)
        {
            ChangeSlotMelee();
            AttackMode();

        }

        if (_hasRangedWeapon)
        {
            AimMode();
            ChangeSlotRanged();
            ShootReset();

        }
    }

    private void ShootReset()
    {
        if(counterShoot<delayShoot) counterShoot += Time.deltaTime;
            counterShoot = counterShoot >= delayShoot ? delayShoot : counterShoot;
    }


    private void AimMode()
    {
        if (aimingDelay == 0)
            return;
        
        fullAim = Mathf.Approximately(aimingCounter ,aimingDelay);

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

    
    private void AttackMode()
    {
        inTransition = attackModeCounter != 0 && !Mathf.Approximately(attackModeCounter ,attackModeDelay);

        if (attackModeDelay == 0)
            return;
        
        if (_attackMode)
        {
            if(attackModeCounter<attackModeDelay) attackModeCounter += Time.deltaTime;
            attackModeCounter = attackModeCounter >= attackModeDelay ? attackModeDelay : attackModeCounter;
        }
        else
        {
            if(attackModeCounter>0) attackModeCounter -= Time.deltaTime;
            attackModeCounter = attackModeCounter <= 0 ? 0 : attackModeCounter;
        }

        characterAnimator.SetLayerWeight(MeleeArmedLayer,attackModeCounter/(attackModeDelay));
        
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
        _isAiming = Input.GetButton("Aim") && _hasRangedWeapon;
        _inputChangeCombat = Input.GetButtonDown("ChangeCombatMode");
        _inputDash = Input.GetButtonDown("Dash");
        _inputJump = Input.GetButtonDown("Jump");

    }

    private void ChangeSlotMelee()
    {
        if (!_hasMeleeWeapon)
            return;
        AttachWeaponToSlot(_attackMode ? ArmedMeleeSlot : UnarmedMeleeSlot,ref _currentSlotMelee,ref meleeWeapon);
    }

    private void ChangeSlotRanged()
    {
        if (!_hasRangedWeapon)
            return;
        AttachWeaponToSlot(_isAiming ? ArmedRangedSlot : UnarmedRangedSlot,ref _currentSlotRanged,ref rangedWeapon);
    }
    

    private void AttachWeaponToSlot(Transform slot, ref Transform currentSlot, ref GameObject weapon )
    {
        currentSlot = slot;
        weapon.transform.parent = currentSlot;
        Vector3 interpolatedPosition = Vector3.Lerp(weapon.transform.position, currentSlot.position,Time.deltaTime * positionSlotSpeed);

        Quaternion interpolatedAngle = Quaternion.Slerp (weapon.transform.rotation, currentSlot.rotation, Time.deltaTime * rotationSlotSpeed);

        weapon.transform.position = interpolatedPosition;
        weapon.transform.rotation = interpolatedAngle;
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
        StartCoroutine(DashRoutine());
    }

    public void GiveMeleeWeaponToPlayer(GameObject meleeWeaponToGive)
    {
        if (meleeWeaponToGive)
        {
            meleeWeapon = Instantiate(meleeWeaponToGive,UnarmedMeleeSlot.position,UnarmedMeleeSlot.rotation);
            AttachWeaponToSlot(_attackMode ? ArmedMeleeSlot : UnarmedMeleeSlot,ref _currentSlotMelee,ref meleeWeapon);
            MeleeWeapon scriptWeapon = meleeWeapon.GetComponent<MeleeWeapon>();
            _meleeWeaponScript = scriptWeapon;
            scriptWeapon.TargetTag = targetTag;
            scriptWeapon.Damage *= damageMulti;
        }
    }
    
    public void GiveRangedWeaponToPlayer(GameObject rangedWeaponToGive)
    {
        if (rangedWeaponToGive)
        {
            rangedWeapon = Instantiate(rangedWeaponToGive,UnarmedRangedSlot.position,UnarmedRangedSlot.rotation);
            AttachWeaponToSlot(_isAiming ? ArmedRangedSlot : UnarmedRangedSlot,ref _currentSlotRanged,ref rangedWeapon);
            RangedWeapon scriptWeapon = rangedWeapon.GetComponent<RangedWeapon>();
            _rangedWeaponScript = scriptWeapon;
            scriptWeapon.TargetTag = targetTag;
            scriptWeapon.DamageMulti *= damageMulti;
        }
    }

    public void GiveDash()
    {
        hasDash = true;
        spriteDash.enabled = true;
    }
    
    public void AddJump()
    {
        numberJumps++;
    }
    
    public void SetDoubleJump()
    {
        numberJumps = 2;
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    IEnumerator SetUpDashSprite()
    {
        float fillCounterDash = dashCoolDown;

        while (fillCounterDash > 0)
        {
            fillCounterDash -= Time.deltaTime;
            if(spriteDash)
                spriteDash.fillAmount = dashCoolDown - fillCounterDash / dashCoolDown;
            yield return null;
        }
        if(spriteDash)
            spriteDash.fillAmount = 1f;
    }

    IEnumerator DashRoutine()
    {
        _isDashing = true;
        currentDashTime = dashTime;
        while (currentDashTime > 0)
        {
            _rb.drag = 0;
            _rb.velocity = new Vector3();
            _rb.AddForce(dashSpeed*Time.deltaTime*transform.forward,ForceMode.VelocityChange);
            currentDashTime -= Time.deltaTime;
            yield return null;
        }
        //print((start - transform.position).magnitude);

        _isDashing = false;
        currentDashCooldown = dashCoolDown;
        StartCoroutine(SetUpDashSprite());
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
