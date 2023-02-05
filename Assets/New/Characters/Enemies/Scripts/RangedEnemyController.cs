using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    /*
    [Header("Aiming")]
    [SerializeField] private bool needToAim;
    [ConditionalField("needToAim")][ReadOnly][SerializeField] 
    private bool _isAiming;
     */
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
    
    
        private void AnimationBehavior()
    {
        
        if (enemyAnimator == null)
        {
            Debug.LogWarning("No Animator Enemy on "+name);
            return;  
        }
        //enemyAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
        
        enemyAnimator.SetFloat(VelocityHash,velocity);
        enemyAnimator.SetFloat(AnimationSpeedAttack, animationSpeedAttack);

        // enemyAnimator.SetBool(IsJumping, _isJumping);
        if(needToAim) 
            enemyAnimator.SetBool(IsAiming,_isAiming);

        enemyAnimator.SetBool(IsAttacking,_isAttacking);
    }
    
    */
}
