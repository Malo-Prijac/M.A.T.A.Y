using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : EnemyControllerBase
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
        
        if (CanAttack(true) && PlayerInRangeToAttack(rangeAttack))
        {
            Attack(attackTag);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
}
