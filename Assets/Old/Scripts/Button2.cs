using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button2 : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private List<GameObject> _linkedDoors;
    [SerializeField] private bool _defaultState = false;

    private bool _isOn = false;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject pressE;

    public PlayerController playerController;

    private void Start()
    {
        _isOn = _defaultState;
        _meshRenderer.material = _offMaterial;
        foreach (GameObject go in _linkedDoors)
        {
            go.GetComponent<Animator>().enabled = false;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && (other.CompareTag("Player")))
        {
            if (_isOn == false)
            {
                _meshRenderer.material = _onMaterial;
                foreach (GameObject go in _linkedDoors)
                {
                    go.GetComponent<Animator>().enabled = true;
                }
                pressE.SetActive(false);
                cam1.SetActive(false);
                cam2.SetActive(true);
                _isOn = true;
                playerController.Change();
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