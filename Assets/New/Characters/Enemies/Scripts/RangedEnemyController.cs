using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyControllerBase
{
    [Header("Aiming")]
    [ReadOnly][SerializeField] private bool _isAiming;
    private static readonly int IsAiming = Animator.StringToHash("IsAiming");

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        AnimationBehavior();
        
        _isAiming = PlayerInRangeToAttack();
        if (CanAttack(_isAiming) && PlayerInRangeToAttack())
        {
            Attack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    protected override void Attack()
    {
        base.Attack();
    }
    
    
    
    
    protected override void AnimationBehavior()
    {
        base.AnimationBehavior();
        enemyAnimator.SetBool(IsAiming,_isAiming);
    }
    
}
