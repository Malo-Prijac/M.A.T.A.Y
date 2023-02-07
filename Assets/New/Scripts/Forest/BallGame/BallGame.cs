using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGame : MonoBehaviour
{
    [SerializeField] private Canvas text;
    [SerializeField] private PlayerController playerController;
    public bool _isOn = false;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isOn == false)
            {
                text.enabled = true;
                if (Input.GetKey(KeyCode.E))
                {
                    text.enabled = false;
                    _isOn = true;
                    playerController.Change();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = false;
        }
    }
}
