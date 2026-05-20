using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTreeStatue : MonoBehaviour
{
    [SerializeField] private GameObject Tree;
    [SerializeField] private GameObject Statue;
    [SerializeField] private GameObject DisabledStatue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Tree.SetActive(false);
            Statue.SetActive(true);
            DisabledStatue.SetActive(false);
        }
    }
}
