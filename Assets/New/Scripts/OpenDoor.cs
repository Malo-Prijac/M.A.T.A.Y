using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            {
                Destroy(door);
            }
    }
}
