using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private GameManager _gameManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.orb == 1)
            {
                _gameManager.currentSpawn = _gameManager.spawnWorld2;
                other.gameObject.transform.position = _gameManager.currentSpawn;
            }
        }
    }
}
