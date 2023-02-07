using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private int relicMin;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject currentWorld;
    [SerializeField] private GameObject newWorld;
    [SerializeField] private PlayerCharacterController _playerCharacterController;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager = GameManager.Instance;
            
            if (_gameManager.relic >= relicMin)
            {
                newWorld.SetActive(true);
                currentWorld.SetActive(false);
                _gameManager.currentSpawn = respawnPoint;
                other.gameObject.transform.position = spawnPoint.position;
                _playerCharacterController.collisions = 0;
            }
        }
    }
}
