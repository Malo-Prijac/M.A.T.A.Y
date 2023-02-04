using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas orbFound;
    [SerializeField] private GameObject portal;

    private GameManager _gameManager;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.orb == 0)
            {
                _gameManager.orb ++;
                portal.transform.GetChild(3).gameObject.SetActive(true);
                portal.transform.GetChild(4).gameObject.SetActive(true);
                interraction.enabled=false;
                orbFound.enabled=true;
            }
            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.orb == 0)
            {
                interraction.enabled = true;
            }
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
