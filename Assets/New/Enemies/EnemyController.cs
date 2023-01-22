using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Animator enemyAnimator;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    //private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    [Header("Enemy Weapon Slots")] 
    [SerializeField] private bool HasDifferentSlotForWeapon;
    [ConditionalField("HasDifferentSlotForWeapon")][SerializeField] 
    private Transform weaponSlotMovement;
    [ConditionalField("HasDifferentSlotForWeapon")][SerializeField] 
    private Transform weaponSlotAttack;
    [ConditionalField("HasDifferentSlotForWeapon")][SerializeField] 
    private float rotationSlotSpeed = 10;
    [ConditionalField("HasDifferentSlotForWeapon")][SerializeField] 
    private float positionSlotSpeed = 10;

    [Header("Player Tag")]
    [ReadOnly] [SerializeField] private string playerTag ="Player";

    [Header("Detect Player")]
    [SerializeField] private float rangePlayer = 5f;
    [SerializeField]private float rotationSpeed = 50;
    
    [ReadOnly] [SerializeField] private bool isPlayerInSight;
    [ReadOnly] [SerializeField] private bool isPlayerInRange;

    
    [Header("Enemy Movement")]
    
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float ratioRootMotion = 2f;
    
    [SerializeField] private float groundDrag;
    [ReadOnly][SerializeField]private float velocity;
    [ReadOnly][SerializeField]private bool _isRunning;

    [Header("Enemy Attack")] 
    [SerializeField] private float delayAttack = 0.5f;
    [SerializeField] private float rangeAttack = 0.5f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private bool hasStaticAttack = false;

    [ReadOnly] [SerializeField] private bool _isAttacking;
    [ReadOnly] [SerializeField] private bool _isPlayerInRangeToAttack;

    [SerializeField] private GameObject weaponType;
    
    private GameObject _weaponGameObject;
    private Weapon _weapon;
    private Transform _currentSlot;

    private float _actualSpeed;
    private Rigidbody _rb;
    private Vector3 _rigidbodyDrag;
    private bool _inMotion;



    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindWithTag(playerTag);
        _weaponGameObject = Instantiate(weaponType,new Vector3(),new Quaternion());
        _weapon = _weaponGameObject.GetComponent<Weapon>();
        AttachWeaponToSlot(weaponSlotMovement);

        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationBehavior();

        if (_player == null)
        {
            Debug.LogWarning("No Player set");
            return;
        }
        
        ChangeSlot();

        if (PlayerInRangeToAttack() && !_isAttacking)
        {
            _inMotion = false;
            Attack();
        }

        


    }
    
    private void FixedUpdate()
    {
        UpdateVelocity();
        FollowPlayer(_player.transform.position-transform.position);

        if (IsPlayerDetected() && !_isAttacking)
        {
            _inMotion = true;
            _isRunning = true;
            LookAtPlayer();
        }
        else
        {
            _isRunning = false;
        }
    }

    private void ChangeSlot()
    {
        if (!HasDifferentSlotForWeapon)
            return;
        if (_isAttacking)
        {
            AttachWeaponToSlot(weaponSlotAttack);
        }
        else if(!_isAttacking)
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
        bool detectedPlayer = IsPlayerDetected();
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
        Quaternion rotation = Quaternion.LookRotation(_player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    
    void Attack()
    {
        _isAttacking = true;
        if (_weapon)
        {
            _weapon.Attack();
        }
        else
        {
            Debug.LogWarning(name+" has no weapon");
        }
        StartCoroutine(StopAttack());
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        _isAttacking = false;
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
        
       // enemyAnimator.SetBool(IsJumping, _isJumping);


       enemyAnimator.SetBool(IsAttacking,_isAttacking);
    }
}

