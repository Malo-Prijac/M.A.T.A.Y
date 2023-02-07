using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCollider : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private GameManager.BiomeType biome;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        _gameManager.Biome = biome;
    }
}
