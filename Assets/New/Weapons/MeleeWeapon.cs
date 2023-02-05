using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{    
    [Header("Weapon Stats")]
    [SerializeField]protected float damage = 10f;
    [SerializeField] protected float delayDamage = 1.2f;
    private bool _applyDamage = true;

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
        
    }
    
    public override void Attack(Vector3 targetPosition, float delaySoundAttack)
    {
        base.Attack(targetPosition,delaySoundAttack);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_applyDamage)
            return;
        
        //if(attackSound)
        
        if (other.gameObject.CompareTag("Player"))
        {

            HealthSystem playerHealthSystem = other.gameObject.GetComponent<HealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(Damage, "skeleton");
            
        }
        _applyDamage = false;
        StartCoroutine(AttackDelaying());
    }
    
    private IEnumerator AttackDelaying()
    {
        yield return new WaitForSeconds(delayDamage);
        _applyDamage = true;
    }
}
