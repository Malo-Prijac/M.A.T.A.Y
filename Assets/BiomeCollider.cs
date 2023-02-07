using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCollider : MonoBehaviour
{
    private GameManager _gameManager;
    private AudioManager _audioManager;
    [SerializeField] private GameManager.BiomeType biome;
    [SerializeField] private Sound BiomeSound;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _audioManager = AudioManager.instance;
        _audioManager.AddNewSound(BiomeSound,_audioManager.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            if (_gameManager.Biome != biome)
            {
                _gameManager.ChangeBiome(biome,BiomeSound);
                print(biome);
            }
    }
}
