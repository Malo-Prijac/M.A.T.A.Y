using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Animator enemyAnimator;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    //private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int AnimationSpeedAttack = Animator.StringToHash("AnimationSpeedAttack");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    [Header("Enemy Weapon Slots")]
    [SerializeField] 
    private Transform weaponSlotMovement;
    [SerializeField] 
    private Transform weaponSlotAttack;
    [SerializeField] 
    private float rotationSlotSpeed = 10;
    [SerializeField] 
    private float positionSlotSpeed = 10;

    [Header("Player Tag")]
    [ReadOnly] [SerializeField] private string playerTag ="Player";

    [Header("Detect Player")]
    [SerializeField] private float rangePlayer = 5f;
    [SerializeField]private float rotationSpeed = 40;
    
    [ReadOnly] [SerializeField] private bool isPlayerInSight;
    [ReadOnly] [SerializeField] private bool isPlayerInRange;

    [Header("Enemy sounds")]
    [SerializeField] private Sound destroySound;

    
    [Header("Enemy Movement")]
    
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float acceleration = 0.8f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float ratioRootMotion = 2f;
    [SerializeField] private float groundDrag = 48;
    [ReadOnly][SerializeField]private float velocity;
    [ReadOnly][SerializeField]private bool _isRunning;

    [Header("Aiming")]
    [SerializeField] private bool needToAim;
    [ConditionalField("needToAim")][ReadOnly][SerializeField] 
    private bool _isAiming;

    [Header("Enemy Attack")] 
    
    [SerializeField] private bool hasStaticAttack;
    [SerializeField] private string attackTag = "Attack";
    [SerializeField] private float delayAttack = 1f;
    [SerializeField] private float animationSpeedAttack = 1f;
    [SerializeField] private float rangeAttack = 1.35f;

    [ReadOnly] [SerializeField] private float _lastAttack;
    [ReadOnly] [SerializeField] private bool _isAttacking;
    [ReadOnly] [SerializeField] private bool _isPlayerInRangeToAttack;
    [ReadOnly] [SerializeField] private bool canAttack;

    [SerializeField] private GameObject weaponType;
    
    [Header("Sounds")]
    [SerializeField] protected float delaySoundAttack;

    [SerializeField] protected Sound movingSound;


    private GameObject _weaponGameObject;
    private Weapon _weapon;
    private Transform _currentSlot;

    private float _actualSpeed;
    private Rigidbody _rb;
    private Vector3 _rigidbodyDrag;
    private bool _inMotion;

    private GameObject _player;
    private Vector3 _targetPosition;
    private Vector3 _offSetPlayer;

    private AudioManager _audioManager;

    void Start()
    {
        _player = GameObject.FindWithTag(playerTag); 
        _offSetPlayer = new Vector3(0,_player.GetComponent<CapsuleCollider>().height/2,0);
        if (weaponType)
        {
            _weaponGameObject = Instantiate(weaponType,new Vector3(),new Quaternion());
            _weapon = _weaponGameObject.GetComponent<Weapon>();
            _weapon.Owner = gameObject;
            AttachWeaponToSlot(weaponSlotMovement);
        }
        else
        {
            Debug.LogWarning(name+" has no weapon type. ");
        }

        _audioManager = AudioManager.instance;

        if (movingSound.clip)
        {
            _audioManager.AddNewSound(movingSound, gameObject);
        }
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            Debug.LogWarning("No Player set");
            return;
        }
        
        #if UNITY_EDITOR
        Assert.IsFalse(rangePlayer < rangeAttack);
        #endif

        AnimationBehavior();
        ChangeSlot();
        UpdateSound();

        if (PlayerInRangeToAttack())
        {
            if (needToAim)
            {
                Aim();
            }
            if(canAttack)
            {
                Attack();
            }
        }
        else
        {
            _isAiming = false;
        }

        if (needToAim)
        {
            if (!_isAttacking && _isAiming)
            {
                _lastAttack+= Time.deltaTime;
            }
        }
        else
        {
            if (!_isAttacking && !canAttack)
            {
                _lastAttack+= Time.deltaTime;
            }
        }

        canAttack = _lastAttack - delayAttack > 0;
    }

    private void UpdateSound()
    {
        if (movingSound.clip)
        {
            if (_inMotion && !movingSound.Source.isPlaying)
            {
                _audioManager.Play(movingSound);
            }
            if (!_inMotion)
            {
                movingSound.Source.Stop();
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        UpdateVelocity();
        FollowPlayer(_player.transform.position-transform.position);

        if (IsPlayerDetected())
        {
            LookAtPlayer();
            if (PlayerInRangeToAttack() || _isAttacking)
            {
                _inMotion = false;
                _isRunning = false;
            }
            else
            {
                _inMotion = true;
                _isRunning = true;
            }
            
        }
        else
        {
            _inMotion = false;
            _isRunning = false;
        }
    }

    /*
    IEnumerator CorrectAttackingPosition()
    {
        while (_isAttacking)
        {
            Vector3 interpolation = new Vector3(0,_offsetRotationYCounterAttack,0);
            _offsetRotationYCounterAttack = Mathf.Lerp(_offsetRotationYCounterAttack, offsetRotationY, Time.deltaTime * rotationSpeedAttack);
            interpolation = new Vector3(0, _offsetRotationYCounterAttack, 0) - interpolation;
            transform.rotation = Quaternion.Euler(transform.eulerAngles + interpolation );
            //transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0,Time.deltaTime * rotationSlotSpeed,0));
            yield return null;
        }

        _offsetRotationYCounterAttack = 0;
    }

    IEnumerator CorrectAimingPosition()
    {
        while (_isAiming){
            Vector3 interpolation = new Vector3(0,_offsetRotationYCounterAiming,0);
            _offsetRotationYCounterAiming = Mathf.Lerp(_offsetRotationYCounterAiming, offsetAiming, Time.deltaTime * rotationSpeedAiming);
            interpolation = new Vector3(0, _offsetRotationYCounterAiming, 0) - interpolation;
            transform.rotation = Quaternion.Euler(transform.eulerAngles + interpolation );
            //transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0,Time.deltaTime * rotationSlotSpeed,0));
            yield return null;
        }

        _offsetRotationYCounterAiming = 0;
    }
    */
    
    private void ChangeSlot()
    {
        if (_isAttacking && weaponSlotAttack)
        {
            AttachWeaponToSlot(weaponSlotAttack);
        }
        else if(!_isAttacking && weaponSlotMovement)
        {
            AttachWeaponToSlot(weaponSlotMovement);
        }
    }
    
    void AttachWeaponToSlot(Transform slot)
    {
        _currentSlot = slot;
        _weaponGameObject.transform.parent = _currentSlot;
        Vector3 interpolatedPosition = Vector3.Lerp(_weaponGameObject.transform.position, _currentSlot.position,Time.deltaTime * positionSlotSpeed);

        Quaternion interpolatedAngle = Quaternion.Slerp (_weaponGameObject.transform.rotation, _currentSlot.rotation, Time.deltaTime * rotationSlotSpeed);

        _weaponGameObject.transform.position = interpolatedPosition;
        _weaponGameObject.transform.rotation = interpolatedAngle;
    }

    private bool IsPlayerDetected()
    {
        isPlayerInRange = (Mathf.Abs((_player.transform.position - transform.position).magnitude) < rangePlayer);
        return isPlayerInRange || isPlayerInSight;
    }
    
    private bool PlayerInRangeToAttack()
    {
        _isPlayerInRangeToAttack = (Mathf.Abs((_player.transform.position - transform.position).magnitude) < rangeAttack);
        return _isPlayerInRangeToAttack;
    }

    void FollowPlayer(Vector3 moveDirection)
    {
        _rb.AddForce(_actualSpeed * moveDirection.normalized,ForceMode.VelocityChange);
        _rigidbodyDrag = new Vector3(-_rb.velocity.x, 0, -_rb.velocity.z)*groundDrag;

        /*
        if (!grounded)
        {
            rigidbodyDrag = -_rb.velocity*airDrag;
            _rb.AddForce(rigidbodyDrag*groundDrag, ForceMode.Acceleration);

        }*/
        
        _rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);

    }
    
    private void UpdateVelocity()
    {
        if ((_inMotion && velocity < walkSpeed / runSpeed)|| (_isRunning && velocity < 1.0f))
        {
            velocity += Time.deltaTime * acceleration;
            velocity = IsPlayerDetected() ? Mathf.Min(velocity, 1) : Mathf.Min(velocity, walkSpeed / runSpeed);
        }
        else if((!_inMotion && velocity > 0.0f) || (_isRunning == false && _actualSpeed > walkSpeed))
        {
            velocity -= Time.deltaTime * deceleration;
            velocity = Mathf.Max(velocity, 0);
        }

        _actualSpeed = velocity*runSpeed*ratioRootMotion;
    }
    
    void LookAtPlayer()
    {
        if (hasStaticAttack && _isAttacking)
            return;

        Vector3 playerPosition = new Vector3(_player.transform.position.x,0,_player.transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    void Aim()
    {
        //StartCoroutine(CorrectAimingPosition());
        _isAiming = true;
    }

    void Attack()
    {
        if(_isAttacking)
            return;

        if (needToAim && !_isAiming)
            return;
        
        _isAttacking = true;
        _lastAttack = 0;

        if (_weapon)
        {
            _targetPosition = _player.transform.position + _offSetPlayer;
            _weapon.Attack(_targetPosition,delaySoundAttack);
            //transform.rotation = Quaternion.Euler(transform.eulerAngles - offsetRotation);
        }
        else
        {
            Debug.LogWarning(name+" Weapon has no weapon script");
        }
        
        //StartCoroutine(CorrectAttackingPosition());
        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {
        //print(attackDuration/animationSpeedAttack);
        while (!IsAnimationCurrentAnimation(attackTag))
        {
            yield return null;
        }
        while (AnimatorIsPlaying(attackTag) )
        {
            yield return null;
        }
        
        _isAttacking = false;
    }

    bool IsAnimationCurrentAnimation(string tagAnim)
    {
       // print(enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag(tagAnim));
        return enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag(tagAnim);
    }

    bool AnimatorIsPlaying(string tagAnim){
        return AnimatorIsPlaying() && IsAnimationCurrentAnimation(tagAnim);
    }
    
    bool AnimatorIsPlaying(){
        //print(enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.99f);
        return enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 < 0.95f;
    }

    private void AnimationBehavior()
    {
        
        if (enemyAnimator == null)
        {
            Debug.LogWarning("No Animator Enemy on "+name);
            return;  
        }
        //enemyAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
        
        enemyAnimator.SetFloat(VelocityHash,velocity);
        enemyAnimator.SetFloat(AnimationSpeedAttack, animationSpeedAttack);

        // enemyAnimator.SetBool(IsJumping, _isJumping);
        if(needToAim) 
            enemyAnimator.SetBool(IsAiming,_isAiming);

        enemyAnimator.SetBool(IsAttacking,_isAttacking);
    }
}

