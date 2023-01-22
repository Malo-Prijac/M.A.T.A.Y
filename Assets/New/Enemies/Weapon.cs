using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField]private float damage;

    [SerializeField] private bool IsFiringProjectiles;
    
    [ConditionalField("IsFiringProjectiles")][SerializeField] private GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (IsFiringProjectiles)
        {
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            PlayerHealthSystem playerHealthSystem = other.gameObject.GetComponent<PlayerHealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(damage);
            
        }
    }
}

