using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas ringFound;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_gameManager.stateRingQuest==1)
        {
            if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
            {
                _gameManager.stateRingQuest = 2;
                interraction.enabled=false;
                ringFound.enabled=true;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameManager.stateRingQuest==1)
        {
            if (other.CompareTag("Player"))
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
            ringFound.enabled = false;
        }
    }
}
