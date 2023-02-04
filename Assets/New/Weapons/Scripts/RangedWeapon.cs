using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Weapon Stats")]
    [SerializeField]protected float damageMulti = 1f;
    
    [Header("Projectile")]
    [SerializeField] 
    private float velocityProjectile;
    [SerializeField] 
    private float airDragY;
    [SerializeField] 
    private float delayThrowProjectile = 0.2f;
    [SerializeField]
    private GameObject projectile;
    
    public float DamageMulti
    {
        get => damageMulti;
        set => damageMulti = value;
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
    
    public override void Attack(Vector3 targetPosition, float delaySoundAttack, Vector3 startPosition = default(Vector3))
    {
        base.Attack(targetPosition, delaySoundAttack);
        StartCoroutine(ThrowProjectile(targetPosition, startPosition));
    }
    
    IEnumerator ThrowProjectile(Vector3 targetPosition, Vector3 startPosition = default(Vector3))
    {
        if (startPosition == Vector3.zero)
            startPosition = transform.position;
        yield return new WaitForSeconds(delayThrowProjectile);
        GameObject projectileGameObject = Instantiate(projectile, startPosition,  
            transform.rotation);
        
        projectileGameObject.transform.LookAt(targetPosition);
        Projectile projectileScript = projectileGameObject.GetComponent<Projectile>();
        projectileScript.Damage *= DamageMulti;
        projectileScript.Owner = gameObject;

        Quaternion aim = Quaternion.LookRotation( targetPosition - transform.position).normalized;
        
        //projectileGameObject.transform.rotation = 
        Rigidbody rb = projectileGameObject.GetComponent<Rigidbody>();
        rb.AddForce(aim*Vector3.forward*velocityProjectile,ForceMode.VelocityChange);
        Vector3 _rigidbodyDrag = Vector3.up*airDragY;
        rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);
        
    }
}
