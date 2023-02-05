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
        
        _isAiming = PlayerInRangeToAttack(rangeAttack);
        if (CanAttack(_isAiming) && _isAiming)
        {
            Attack(attackTag);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }




    protected override void AnimationBehavior()
    {
        base.AnimationBehavior();
        enemyAnimator.SetBool(IsAiming,_isAiming);
    }
    
}
