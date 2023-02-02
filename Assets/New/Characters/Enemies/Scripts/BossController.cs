using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyControllerBase
{
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
        
        /*
        if (CanAttack(true) && PlayerInRangeToAttack())
        {
            Attack();
        }
        */
    }
    
    protected override void FixedUpdate()
    {
        UpdateVelocity();
        FollowPlayer(_player.transform.position-transform.position);
        if (IsPlayerDetected())
        {
            LookAtPlayer();
        }
        
        _inMotion = IsPlayerDetected() && !(PlayerInRangeToAttack() && hasStaticAttack || _isAttacking);
        _isRunning = _inMotion;
        /*
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
        */
    }
    private void AttackSwipping()
    {
        
    }
    
    private void AttackJumping()
    {
        
    }
}
