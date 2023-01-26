using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnigmeMain : MonoBehaviour
{
    [SerializeField] private GameObject pressO;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject dialogue2;
    [SerializeField] private GameObject enemy;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            pressO.SetActive(false);
            PlayerCharacterController playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            if (playerCharacterController.bague)
            {
                dialogue2.SetActive(true);
                enemy.SetActive(true);
            }
            else
            {
                dialogue.SetActive(true);
            }
            
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
        dialogue2.SetActive(false);
    }
}
