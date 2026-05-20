using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintText : MonoBehaviour
{
    [SerializeField] private Canvas text;
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = false;
        }
    }
}
