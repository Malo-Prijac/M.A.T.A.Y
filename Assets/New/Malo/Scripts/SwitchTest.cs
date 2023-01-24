using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTest : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private List<GameObject> _linkedDoors;
    [SerializeField] private float delayDoor;

    private static readonly int ButtonOn = Animator.StringToHash("ButtonOn");
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
            StartCoroutine(MoveGrille());
        }
    }
    
    IEnumerator MoveGrille()
    {
        foreach (GameObject go in _linkedDoors)
        {
            Debug.Log(go);
            go.GetComponent<Animator>().enabled = true;
            go.GetComponent<Animator>().SetBool(ButtonOn, true);
            yield return new WaitForSeconds(delayDoor);
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
                StartCoroutine(MoveGrilleDown());
            }
        }
    }
    
    IEnumerator MoveGrilleDown()
    {
        foreach (GameObject go in _linkedDoors)
        {
            Debug.Log(go);
            go.GetComponent<Animator>().SetBool(ButtonOn, false);
            yield return new WaitForSeconds(delayDoor);
        }
    }
}
