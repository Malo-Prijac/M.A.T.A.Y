using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Canvas interraction;
    [SerializeField] private Canvas relicFound;
    [SerializeField] private GameObject relic;
    [SerializeField] private Sound victorySound;

    private AudioManager _audioManager;
    private GameManager _gameManager;

    private void Start()
    {
        _audioManager = AudioManager.instance;
        _gameManager = GameManager.Instance;
        _audioManager.AddNewSound(victorySound,gameObject);

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.relic == 1)
            {
                _gameManager.relic ++;
                interraction.enabled=false;
                relicFound.enabled=true;
                relic.SetActive(false);
                _audioManager.Play(victorySound);
            }
            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_gameManager.relic == 1)
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
