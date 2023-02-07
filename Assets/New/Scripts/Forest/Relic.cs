using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas relicFound;
    [SerializeField] private GameObject relic;

    private GameManager _gameManager;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.relic == 0)
            {
                _gameManager.relic ++;
                interraction.enabled=false;
                relicFound.enabled=true;
                relic.SetActive(false);
            }
            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.relic == 0)
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
            relicFound.enabled = false;
        }
    }
}
