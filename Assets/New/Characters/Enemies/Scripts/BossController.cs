using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyControllerBase
{
    [Header("Attack Swiping")]
    [ReadOnly][SerializeField] private bool _attackSwiping;
    private static readonly int AttackSwiping = Animator.StringToHash("AttackSwiping");
    [SerializeField]private string attackMeleeTag = "AttackSwiping";
    
    [Header("Attack Jump")]
    [ReadOnly][SerializeField] private bool _attackJumping;
    [SerializeField]private float rangeJumpingAttack = 4;
    private static readonly int AttackJumping = Animator.StringToHash("AttackJump");
    [SerializeField]private string attackJumpingTag = "AttackJumping";
    [ReadOnly][SerializeField]private float _lastAttackJumping = 0;
    [SerializeField]private float delayAttackJumping = 10;

    [SerializeField] private float jumpSpeed = 8;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        AnimationBehavior();
        
        
        if (CanAttack(true))
        {
            if(PlayerInRangeToAttack(rangeAttack))
                SwipingAttack();
            
            else if(CanJumpAttack(true))
                if (PlayerInRangeToAttack(rangeJumpingAttack))
                {
                    JumpingAttack();
                }
        }


        _attackSwiping = _attackSwiping && _isAttacking;
        _attackJumping = _attackJumping && _isAttacking;

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //MoveJumpingAttack();
    }

    private void SwipingAttack()
    {
        _attackSwiping = true;
        Attack(attackMeleeTag);
    }
    
    private void JumpingAttack()
    {
        _lastAttackJumping = 0;
        _attackJumping = true;
        Attack(attackJumpingTag);
    }

    private bool CanJumpAttack(bool condition)
    {
        if (!_attackJumping && condition)
            _lastAttackJumping+= Time.deltaTime;

        return _lastAttackJumping - delayAttackJumping > 0;
    }
    
    protected override void AnimationBehavior()
    {
        base.AnimationBehavior();
        enemyAnimator.SetBool(AttackSwiping,_attackSwiping);
        enemyAnimator.SetBool(AttackJumping,_attackJumping);
    }
    
    /*
    private void MoveJumpingAttack()
    {
        if (!_attackJumping)
            return;
        
        Vector3 moveDirection =_player.transform.position - transform.position;
        moveDirection.y = 0;
        _actualSpeed = velocity*jumpSpeed;
        _rb.AddForce(_actualSpeed * moveDirection.normalized,ForceMode.VelocityChange);
        _rigidbodyDrag = new Vector3(-_rb.velocity.x, 0, -_rb.velocity.z)*groundDrag;

        _rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);
    }
    */
}
