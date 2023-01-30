using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GiveShoot : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas tuto;
    private PlayerCharacterController playerCharacterController;
    private bool giveShoot = false;
    
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            speakStatue.enabled=false;
            dialogue.enabled=true;
            giveShoot = true;
            //DONNER LE SHOOT ICI
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
            if (giveShoot)
            {
                giveShoot = false;
                StartCoroutine(TutoDash());
            }
        }
    }
    
    IEnumerator TutoDash()
    {
        tuto.enabled = true;
        yield return new WaitForSeconds(3f);
        tuto.enabled = false;
    }
}