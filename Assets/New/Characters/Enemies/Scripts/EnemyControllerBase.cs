using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyControllerBase : CharacterControllerBase
{
    [SerializeField] public Animator enemyAnimator;
    //private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int AnimationSpeedAttack = Animator.StringToHash("AnimationSpeedAttack");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    [Header("Enemy Weapon Slots")]
    
    [SerializeField] 
    private bool NeedWeaponSlot = true;
    
    [ConditionalField("NeedWeaponSlot")][SerializeField] 
    private Transform weaponSlotMovement;
    [ConditionalField("NeedWeaponSlot")][SerializeField] 
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
    
    [SerializeField] protected float walkSpeed = 1.5f;
    [SerializeField] protected float runSpeed = 3f;
    [SerializeField] protected float acceleration = 0.8f;
    [SerializeField] protected float deceleration = 2f;
    [SerializeField] protected float groundDrag = 48;
    [ReadOnly][SerializeField]protected float velocity;
    [ReadOnly][SerializeField]protected bool _isRunning;
    [ReadOnly][SerializeField]protected bool _inMotion;
    [SerializeField] protected float rangeMinToFollowPlayer = 0.8f;

    [Header("Enemy Attack")] 
    
    [SerializeField] private string targetTag = "Player";
    [SerializeField] protected bool hasStaticAttack;
    [SerializeField] protected string attackTag = "Attack";
    [SerializeField] protected float delayAttack = 1f;
    [SerializeField] protected float animationSpeedAttack = 1f;
    [SerializeField] protected float rangeAttack = 1.35f;

    [ReadOnly] [SerializeField] protected float _lastAttack;
    [ReadOnly] [SerializeField] protected bool _isAttacking;
    [ReadOnly] [SerializeField] protected bool _isPlayerInRangeToAttack;
    [ReadOnly] [SerializeField] protected bool canAttack;

    [SerializeField] protected GameObject weaponType;
    
    [Header("Sounds")]
    [SerializeField] protected float delaySoundAttack;

    [SerializeField] protected Sound movingSound;


    protected GameObject _weaponGameObject;
    protected Weapon _weapon;
    protected Transform _currentSlot;

    protected float _actualSpeed;
    protected Rigidbody _rb;
    protected Vector3 _rigidbodyDrag;

    protected GameObject _player;
    protected Vector3 _targetPosition;
    protected Vector3 _offSetPlayer;
    

    private AudioManager _audioManager;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _player = GameObject.FindWithTag(playerTag); 
        _offSetPlayer = new Vector3(0,_player.GetComponent<CapsuleCollider>().height/2,0);
        if (weaponType)
        {
            _weaponGameObject = Instantiate(weaponType,new Vector3(),new Quaternion());
            _weapon = _weaponGameObject.GetComponent<Weapon>();
            _weapon.Owner = gameObject;
            AttachWeaponToSlot(weaponSlotMovement);
            _weapon.TargetTag = targetTag;
        }

        _audioManager = AudioManager.instance;

        if (movingSound.clip)
        {
            _audioManager.AddNewSound(movingSound, gameObject);
        }
        _rb = GetComponent<Rigidbody>();

    }
    
    protected virtual void Update()
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

    }
    
    protected bool CanAttack(bool condition)
    {
        if (!_isAttacking && condition)
            _lastAttack+= Time.deltaTime;

        return _lastAttack - delayAttack > 0;
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
        if (IsPlayerDetected())
            LookAtPlayer();

        _inMotion = isInMotion();
        _isRunning = _inMotion;
        
        UpdateVelocity();
        if (_inMotion)
        {
            Vector3 positionToFollow =_player.transform.position - transform.position;
            positionToFollow.y = 0;
            FollowPlayer(positionToFollow);

        }
    }

    protected virtual bool isInMotion()
    {
        return IsPlayerDetected() && !(PlayerInRangeToAttack(rangeAttack) && hasStaticAttack || _isAttacking) &&  (PlayerInRangeToFollow());
    }

    private void ChangeSlot()
    {
        switch (_isAttacking)
        {
            case true when weaponSlotAttack:
                AttachWeaponToSlot(weaponSlotAttack);
                break;
            case false when weaponSlotMovement:
                AttachWeaponToSlot(weaponSlotMovement);
                break;
        }
    }
    
    private void AttachWeaponToSlot(Transform slot)
    {
        _currentSlot = slot;
        _weaponGameObject.transform.parent = _currentSlot;
        Vector3 interpolatedPosition = Vector3.Lerp(_weaponGameObject.transform.position, _currentSlot.position,Time.deltaTime * positionSlotSpeed);

        Quaternion interpolatedAngle = Quaternion.Slerp (_weaponGameObject.transform.rotation, _currentSlot.rotation, Time.deltaTime * rotationSlotSpeed);

        _weaponGameObject.transform.position = interpolatedPosition;
        _weaponGameObject.transform.rotation = interpolatedAngle;
    }

    protected bool IsPlayerDetected()
    {
        isPlayerInRange = (Mathf.Abs((_player.transform.position - transform.position).magnitude) < rangePlayer);
        return isPlayerInRange || isPlayerInSight;
    }
    
    protected bool PlayerInRangeToAttack(float range)
    {
        _isPlayerInRangeToAttack = (Mathf.Abs((_player.transform.position - transform.position).magnitude) < range);
        return _isPlayerInRangeToAttack;
    }
    
    protected bool PlayerInRangeToFollow()
    {
        _isPlayerInRangeToAttack = (Mathf.Abs((_player.transform.position - transform.position).magnitude) > rangeMinToFollowPlayer);
        return _isPlayerInRangeToAttack;
    }


    protected void FollowPlayer(Vector3 moveDirection)
    {
        _rb.AddForce(_actualSpeed * moveDirection.normalized,ForceMode.VelocityChange);
        _rigidbodyDrag = new Vector3(-_rb.velocity.x, 0, -_rb.velocity.z)*groundDrag;

        _rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);

    }
    
    protected void UpdateVelocity()
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

        _actualSpeed = velocity*runSpeed;
    }
    
    protected void LookAtPlayer()
    {
        if (hasStaticAttack && _isAttacking)
            return;

        Vector3 playerPosition = new Vector3(_player.transform.position.x,0,_player.transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    virtual protected void Attack(string tagAttack)
    {
        if(_isAttacking)
            return;

        _isAttacking = true;
        _lastAttack = 0;

        if (_weapon)
        {
            _targetPosition = _player.transform.position + _offSetPlayer;
            _weapon.Attack(_targetPosition,delaySoundAttack);
            //transform.rotation = Quaternion.Euler(transform.eulerAngles - offsetRotation);
        }

        //StartCoroutine(CorrectAttackingPosition());
        StartCoroutine(StopAttack(tagAttack));
    }

    private IEnumerator StopAttack(string tagAnim)
    {
        while (!HelperAnimation.IsAnimationCurrentAnimation(enemyAnimator,tagAnim))
        {
            yield return null;
        }
        while (HelperAnimation.AnimatorIsPlaying(enemyAnimator,tagAnim) )
        {
            yield return null;
        }
        
        _isAttacking = false;
    }



    protected virtual void AnimationBehavior()
    {
        
        if (enemyAnimator == null)
        {
            Debug.LogWarning("No Animator Enemy on "+name);
            return;  
        }
        //enemyAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
        
        enemyAnimator.SetFloat(VelocityHash,velocity);
        enemyAnimator.SetFloat(AnimationSpeedAttack, animationSpeedAttack);

        enemyAnimator.SetBool(IsAttacking,_isAttacking);
    }
}


