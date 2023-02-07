using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private Sound soundBridge;
    private AudioManager _audioManager;
    [SerializeField] private Animator BridgeAnimator;
    [SerializeField] private GameObject Pillars;


    private void Start()
    {
        _audioManager = AudioManager.instance;
        if (soundBridge.clip)
            _audioManager.AddNewSound(soundBridge,BridgeAnimator.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Pillars.SetActive(true);
        transform.GetChild(1).gameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.GetChild(1).gameObject.transform.localEulerAngles = new Vector3(90, 0, 0);
        BridgeAnimator.enabled = true;
        _audioManager.Play(soundBridge);
    }
}
