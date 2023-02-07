using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private int orbMin;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject currentWorld;
    [SerializeField] private GameObject newWorld;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager = GameManager.Instance;
            
            if (_gameManager.orb >= orbMin)
            {
                newWorld.SetActive(true);
                currentWorld.SetActive(false);
                _gameManager.currentSpawn = respawnPoint;
                other.gameObject.transform.position = spawnPoint.position;
            }
        }
    }
}
