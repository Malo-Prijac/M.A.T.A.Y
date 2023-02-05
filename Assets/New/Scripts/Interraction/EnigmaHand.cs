using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnigmaHand : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas dialogue2;
    [SerializeField] private GameObject enemy;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            speakStatue.enabled=false;
            PlayerCharacterController playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            if (playerCharacterController.bague)
            {
                enemy.SetActive(true);
                dialogue2.enabled=true;
            }
            else
            {
                dialogue.enabled=true;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakStatue.enabled=true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakStatue.enabled = false;
            dialogue.enabled = false;
            dialogue2.enabled = false;
        }
    }
}
