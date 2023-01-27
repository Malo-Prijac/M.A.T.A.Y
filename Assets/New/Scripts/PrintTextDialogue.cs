using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTextDialogue : MonoBehaviour
{
    [SerializeField] private Canvas pressO;
    [SerializeField] private Canvas dialogue;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            pressO.enabled=false;
            dialogue.enabled=true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressO.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressO.enabled = false;
            dialogue.enabled = false;
        }
    }
}
