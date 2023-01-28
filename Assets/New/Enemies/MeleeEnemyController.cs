using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*
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
            _weapon.Attack();
            //transform.rotation = Quaternion.Euler(transform.eulerAngles - offsetRotation);
        }
        else
        {
            Debug.LogWarning(name+" Weapon has no weapon script");
        }
        
        StartCoroutine(CorrectAttackingPosition());
        StartCoroutine(StopAttack());
    }
    */
}
