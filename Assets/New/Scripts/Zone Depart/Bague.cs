using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bague : MonoBehaviour
{
    [SerializeField] private Canvas pressO;
    [SerializeField] private Canvas bagueTrouve;

    private PlayerCharacterController playerCharacterController;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            playerCharacterController.bague = true;
            pressO.enabled=false;
            bagueTrouve.enabled=true;
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
            bagueTrouve.enabled = false;
        }
    }
}
