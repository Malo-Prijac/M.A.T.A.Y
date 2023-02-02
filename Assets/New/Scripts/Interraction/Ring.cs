using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas ringFound;

    private GameManager _gameManager;
    //private PlayerCharacterController playerCharacterController;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            _gameManager = GameManager.Instance;
            _gameManager.ring = true;
            interraction.enabled=false;
            ringFound.enabled=true;
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
            ringFound.enabled = false;
        }
    }
}
