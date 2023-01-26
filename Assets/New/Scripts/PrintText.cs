using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other)
    {
        text.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        text.SetActive(false);
    }
}
