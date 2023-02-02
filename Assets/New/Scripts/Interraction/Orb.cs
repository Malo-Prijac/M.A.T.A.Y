using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas orbFound;
    [SerializeField] private GameObject portal;

    private PlayerCharacterController playerCharacterController;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            playerCharacterController.orb = 1;
            
            //portal.GetComponent<Teleportation>().enabled = true;
            portal.transform.GetChild(3).gameObject.SetActive(true);
            portal.transform.GetChild(4).gameObject.SetActive(true);
            interraction.enabled=false;
            orbFound.enabled=true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interraction.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interraction.enabled = false;
            orbFound.enabled = false;
        }
    }
}
