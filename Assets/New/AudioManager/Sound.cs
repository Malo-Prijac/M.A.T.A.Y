using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{

    public string name;
    
    public AudioClip clip;

    [Range(0,1)]
    public float volume = 1f;
    
    [Range(0,1)]
    public float maxVolume = 1f;
    
    [Range(0.1f,5)]
    public float pitch = 1f;

    public SoundType type;

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    [Range(0.1f,15)]
    public float fadingTime;
    
    public AnimationCurve FadingCurve;

    private GameObject owner;
    
    public GameObject Owner
    {
        get => owner;
        set => owner = value;
    }
}