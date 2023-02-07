using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas relicFound;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject portalHubDesert;
    [SerializeField] private GameObject relic;

    private GameManager _gameManager;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.relic == 0)
            {
                _gameManager.relic ++;
                portal.transform.GetChild(3).gameObject.SetActive(true);
                portal.transform.GetChild(4).gameObject.SetActive(true);
                portalHubDesert.transform.GetChild(3).gameObject.SetActive(true);
                portalHubDesert.transform.GetChild(4).gameObject.SetActive(true);
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
