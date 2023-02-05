using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGate : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _onMaterial;
    [SerializeField] private Material _offMaterial;
    [SerializeField] private List<GameObject> _linkedDoors;
    
    [Header("Delay Door")]
    [SerializeField] private float delayDoorOpen;
    [SerializeField] private float delayDoorClose;

    [Header("Animation Speed")]
    [SerializeField] private float speedMoveUp = 1f;
    [SerializeField] private float speedMoveDown = 1f;

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
            StartCoroutine(MoveGrilleUp());
        }
    }
    
    IEnumerator MoveGrilleUp()
    {
        foreach (GameObject go in _linkedDoors)
        {
            Animator animator = go.GetComponent<Animator>();
            animator.enabled = true;
            animator.SetBool(ButtonOn, true);
            animator.speed = speedMoveUp;
            yield return new WaitForSeconds(delayDoorOpen);
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
            Animator animator = go.GetComponent<Animator>();
            animator.enabled = true;
            animator.SetBool(ButtonOn, false);
            animator.speed = speedMoveDown;
            yield return new WaitForSeconds(delayDoorClose);
        }
    }
}
