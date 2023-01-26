using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bague : MonoBehaviour
{
    [SerializeField] private GameObject pressO;
    [SerializeField] private GameObject dialogue;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            PlayerCharacterController playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            playerCharacterController.bague = true;
            pressO.SetActive(false);
            dialogue.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pressO.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        pressO.SetActive(false);
        dialogue.SetActive(false);
    }
}
