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
    public GameObject pressE;
    public GameObject dead;
    public TextMeshProUGUI reasonDead;

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
        
        pressE.SetActive(false);
        dead.SetActive(false);
        reasonDead.enabled=false;
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
            
            pressE.SetActive(false);
            //dead.SetActive(true);
            //reasonDead.enabled=true;
            //reasonDead.text = "Mort car il est con";

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