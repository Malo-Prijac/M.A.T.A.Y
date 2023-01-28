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
    public float volume;
    
    [Range(0,1)]
    public float maxVolume = 1;
    
    [Range(0.1f,5)]
    public float pitch;

    public SoundType type;

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    [Range(0.1f,15)]
    public float fadingTime;
    
    public AnimationCurve FadingCurve;

}