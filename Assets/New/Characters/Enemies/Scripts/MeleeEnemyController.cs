using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : EnemyControllerBase
{
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        AnimationBehavior();
        
        if (CanAttack(true) && PlayerInRangeToAttack())
        {
            Attack();
        }
    }
    
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    
    protected override void Attack()
    {
        base.Attack();
    }
}
