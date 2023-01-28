using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private string bgName;
    [SerializeField] private Sound backgroundSound;
    
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (backgroundSound!=null)
        {
            audioManager.AddNewSound(backgroundSound,gameObject);
            audioManager.Play(backgroundSound);
            audioManager.FadeInAllSound();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}