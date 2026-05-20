using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{    
    [Header("Weapon Stats")]
    [SerializeField]protected float damage = 10f;
    [SerializeField] protected float delayDamage = 1.2f;
    private bool _applyDamage = false;
    private bool _hasAttacked = false;
    public float Damage
    {
        get => damage;
        set => damage = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isAttacking && !_applyDamage && !_hasAttacked)
        {
            _applyDamage = true;
            _hasAttacked = true;
        }

        if (!isAttacking)
            _hasAttacked = false;
        //
    }
    
    public override void Attack(Vector3 targetPosition = default(Vector3), float delaySoundAttack = 0, Vector3 startPosition = default(Vector3))
    {
        base.Attack(targetPosition,delaySoundAttack,startPosition);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_applyDamage || !isAttacking)
            return;
        
        //if(attackSound)
        if (other.gameObject.CompareTag(targetTag) || other.gameObject.CompareTag("Bush"))
        {

            HealthSystem playerHealthSystem = other.gameObject.GetComponent<HealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(Damage, "un skeleton");

        }
        _applyDamage = false;
    }
    
    private IEnumerator AttackDelaying()
    {
        yield return new WaitForSeconds(delayDamage);
        _applyDamage = true;
    }
}
