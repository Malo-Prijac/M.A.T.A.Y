using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<HealthSystem>().currentHealth<100)
            {
                other.GetComponent<HealthSystem>().Heal();
                Destroy(gameObject);
            }
        }
    }
}
