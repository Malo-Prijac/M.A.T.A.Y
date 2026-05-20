using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMaze : MonoBehaviour
{
    [SerializeField] private Transform Door;
    [SerializeField] private Canvas Interraction;
    
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
        {
            Interraction.enabled=false;
            transform.localEulerAngles += new Vector3(180, 180, 0);
            Door.localPosition += new Vector3(0, 7, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interraction.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interraction.enabled = false;
        }
    }
}
