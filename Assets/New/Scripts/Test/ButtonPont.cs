using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ButtonPont : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private List<GameObject> _linkedDoors;
    [SerializeField] private bool _defaultState = false;
    [SerializeField] private GameObject _pillar;
    [SerializeField] private GameObject pressO;

    private bool _isOn = false;

    private void Start()
    {
        _pillar.SetActive(false);
        _isOn = _defaultState;
        _meshRenderer.material = _offMaterial;
        foreach (GameObject go in _linkedDoors)
        {
            go.GetComponent<Animator>().enabled = false;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            if (_isOn == false) {
                _meshRenderer.material = _onMaterial;
                _pillar.SetActive(true);
                foreach (GameObject go in _linkedDoors)
                {
                    go.GetComponent<Animator>().enabled = true;
                }
                _isOn = true;
            }
            
            pressO.SetActive(false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isOn)
        {
            pressO.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pressO.SetActive(false);
    }

}