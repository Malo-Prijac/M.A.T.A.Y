using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CamHand : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogueLeave;
    
    private bool choice = false;
    public GameObject Player;
    public GameObject camPlayer;
    public GameObject camTree;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Action") && (other.CompareTag("Player")))
        {
            speakStatue.enabled=false;
            Change();
        }
    }
    public void Change()
    {
        choice = !choice;
        if (choice)
        {
            Player.GetComponent<PlayerCharacterController>().enabled = false;
            camPlayer.SetActive(false);
            camTree.SetActive(true);
            dialogueLeave.enabled = true;
        }
        else
        {
            Player.GetComponent<PlayerCharacterController>().enabled = true;
            camPlayer.SetActive(true);
            camTree.SetActive(false);
            dialogueLeave.enabled = false;
            speakStatue.enabled=true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakStatue.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakStatue.enabled = false;
        }
    }
}
