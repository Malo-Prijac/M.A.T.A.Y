using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("TO");
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem playerHealthSystem = other.gameObject.GetComponent<HealthSystem>();
            playerHealthSystem.TakeDamage(100,"chute vertigineuse",null);
        }
    }
    
}
