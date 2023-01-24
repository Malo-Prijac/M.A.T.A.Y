using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Player Tag")]
    [ReadOnly] [SerializeField] private string playerTag ="Player";
    [Header("Weapon Stats")]
    [SerializeField]private float damageMulti;

    [SerializeField] private bool IsFiringProjectiles;
    [ConditionalField("IsFiringProjectiles")][SerializeField] 
    private float launchVelocity;
    [ConditionalField("IsFiringProjectiles")][SerializeField] 
    private float airDragY;
    [ConditionalField("IsFiringProjectiles")][SerializeField] 
    private float delayThrowProjectile = 0.2f;
    [ConditionalField("IsFiringProjectiles")][SerializeField] private GameObject projectile;
    
    [Header("Owner")]

    private GameObject _player;
    private float _playerheight;

    [SerializeField][ReadOnly]private GameObject owner;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag(playerTag);
        _playerheight = _player.GetComponent<CapsuleCollider>().height;
    }

    public float DamageMulti
    {
        get => damageMulti;
        set => damageMulti = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Owner
    {
        get => owner;
        set => owner = value;
    }
    public void Attack()
    {
        if (IsFiringProjectiles)
        {
            StartCoroutine(ThrowProjectile());
        }
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
        rb.AddForce(aim*Vector3.forward*launchVelocity,ForceMode.VelocityChange);
        Vector3 _rigidbodyDrag = Vector3.up*airDragY;
        rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            PlayerHealthSystem playerHealthSystem = other.gameObject.GetComponent<PlayerHealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(damageMulti);
            
        }
    }
}

