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
        if (other.CompareTag("Player"))
        {
            _meshRenderer.material = _onMaterial;
            StartCoroutine(MoveGrilleUp());
        }
    }
    
    IEnumerator MoveGrilleUp()
    {
        foreach (GameObject go in _linkedDoors)
        {
            Animator animator = go.GetComponent<Animator>();
            animator.speed = speedMoveUp;
            animator.enabled = true;
            animator.SetBool(ButtonOn, true);
            yield return new WaitForSeconds(delayDoorOpen);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _meshRenderer.material = _offMaterial;
            StartCoroutine(MoveGrilleDown());
        }
    }
    
    IEnumerator MoveGrilleDown()
    {
        foreach (GameObject go in _linkedDoors)
        {
            Animator animator = go.GetComponent<Animator>();
            if (animator.GetBool(ButtonOn))
            {
                animator.speed = speedMoveDown;
                animator.enabled = true;
                animator.SetBool(ButtonOn, false);
            }
            yield return new WaitForSeconds(delayDoorClose);
        }
    }
}
