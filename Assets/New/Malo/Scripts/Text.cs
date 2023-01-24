using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private List<GameObject> _linkedDoors;
    [SerializeField] private bool _defaultState = false;
    public GameObject pressE;

    private bool _isOn = false;

    private void Start()
    {
        _isOn = _defaultState;
        _meshRenderer.material = _offMaterial;
        foreach (GameObject go in _linkedDoors)
        {
            go.GetComponent<Animator>().enabled = false;
        }
        pressE.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            if (_isOn == false) {
                _meshRenderer.material = _onMaterial;
                foreach (GameObject go in _linkedDoors)
                {
                    go.GetComponent<Animator>().enabled = true;
                }
                _isOn = true;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isOn)
        {
            pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pressE.SetActive(false);
    }

}