using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTrap : MonoBehaviour
{
    [SerializeField] private List<TrapArrow> _listTrapArrow = new List<TrapArrow>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            
            foreach (TrapArrow ta in _listTrapArrow)
            {
                StartCoroutine(ta.ActivateTrap(0));
            }
        }
    }
}
