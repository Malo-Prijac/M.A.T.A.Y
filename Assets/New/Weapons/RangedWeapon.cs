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
    
    public override void Attack()
    {
        base.Attack();
        StartCoroutine(ThrowProjectile());
    }
    
    IEnumerator ThrowProjectile()
    {
        yield return new WaitForSeconds(delayThrowProjectile);
        GameObject projectileGameObject = Instantiate(projectile, transform.position,  
            transform.rotation);
        projectileGameObject.GetComponent<Projectile>().Damage *= DamageMulti;
        Quaternion aim = Quaternion.LookRotation(_player.transform.position + new Vector3(0,_playerheight/2,0) - transform.position).normalized;
        
        //projectileGameObject.transform.rotation = 
        Rigidbody rb = projectileGameObject.GetComponent<Rigidbody>();
        rb.AddForce(aim*Vector3.forward*velocityProjectile,ForceMode.VelocityChange);
        Vector3 _rigidbodyDrag = Vector3.up*airDragY;
        rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);
        
    }
}
