using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GiveDash : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas tuto;
    private PlayerCharacterController playerCharacterController;
    private bool giveDash = false;
    
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            playerCharacterController = other.gameObject.GetComponent<PlayerCharacterController>();
            speakStatue.enabled=false;
            dialogue.enabled=true;
            giveDash = true;
            //DONNER LE DASH ICI
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
            if (giveDash)
            {
                giveDash = false;
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