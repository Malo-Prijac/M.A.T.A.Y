using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    //private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    [Header("Enemy Weapon Slots")] 
    public bool hasDifferentSlotForWeapon;
    [HideInInspector] public Transform weaponSlotMovement;
    [HideInInspector] public Transform weaponSlotAttack;

    [Header("Player Tag")]
    [ReadOnly] [SerializeField] private string playerTag ="Player";

    [Header("Detect Player")]
    [SerializeField] private float rangePlayer = 5f;
    [SerializeField]private float rotationSpeed = 50;
    
    [ReadOnly] [SerializeField] private bool isPlayerInSight;
    [ReadOnly] [SerializeField] private bool isPlayerInRange;

    
    [Header("Enemy Movement")]
    
    [Header("Enemy Attack")] 
    [SerializeField] private float delayAttack = 0.5f;
    [SerializeField] private float rangeAttack = 0.5f;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private bool hasStaticAttack = false;

    [ReadOnly] [SerializeField] private bool _isAttacking;
    [ReadOnly] [SerializeField] private bool _isPlayerInRangeToAttack;

    [SerializeField] private Weapon weapon;


    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindWithTag(playerTag);
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
        
        if (IsPlayerDetected())
        {
            FollowPlayer();
            LookAtPlayer();
        }
        
        if (PlayerInRangeToAttack() && !_isAttacking)
        {
            Attack();
        }
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

    void FollowPlayer()
    {
         //Quaternion rotation = Quaternion.LookRotation(_moveDirection);
         //transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * dampingRotation);

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
        if (weapon)
        {
            weapon.Attack();
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
        
        //enemyAnimator.SetFloat(VelocityHash,velocity);
        
       // enemyAnimator.SetBool(IsJumping, _isJumping);


       enemyAnimator.SetBool(IsAttacking,_isAttacking);
    }
}

