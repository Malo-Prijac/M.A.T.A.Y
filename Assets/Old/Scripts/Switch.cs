using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private List<GameObject> _linkedDoors;

    private int compteur = 0;

    private void Start()
    {
        _meshRenderer.material = _offMaterial;
        foreach (GameObject go in _linkedDoors)
        {
            go.GetComponent<Animator>().enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Caisse"))
        {
            compteur++;
            _meshRenderer.material = _onMaterial;
            foreach (GameObject go in _linkedDoors)
            {
                go.GetComponent<Animator>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Caisse"))
        {
            compteur--;
            if (compteur == 0)
            {
                _meshRenderer.material = _offMaterial;
                foreach (GameObject go in _linkedDoors)
                {
                    go.GetComponent<Animator>().enabled = false;
                }
            }
        }
        
    }
}
