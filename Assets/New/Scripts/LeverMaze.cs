using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMaze : MonoBehaviour
{
    [SerializeField] private Transform Door;
    
    private void OnTriggerEnter(Collider other)
    {
        transform.localEulerAngles += new Vector3(180, 180, 0);
        Door.localPosition += new Vector3(0, 7, 0);
    }
}
