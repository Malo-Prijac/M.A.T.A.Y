using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private Animator BridgeAnimator;
    [SerializeField] private GameObject Pillars;
    
    private void OnTriggerEnter(Collider other)
    {
        Pillars.SetActive(true);
        transform.GetChild(1).gameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.GetChild(1).gameObject.transform.localEulerAngles = new Vector3(90, 0, 0);
        BridgeAnimator.enabled = true;
    }
}
