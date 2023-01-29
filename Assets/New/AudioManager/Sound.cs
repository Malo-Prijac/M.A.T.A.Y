using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    [Header("Spatial Sound Settings")]
    [Range(0,1)]public float spatialBlend = 1f;
    public float maxDistance = 1f;
    public float minDistance = 15f;
    
    [Header("Options Sound Settings")]
    public string name;
    public AudioClip clip;
    public bool playOnAwake;
    
    [Header("Volume")]
    [Range(0,1)]public float volume = 1f;
    [Range(0,1)] public float maxVolume = 1f;
    [Range(0.1f,5)]
    public float pitch = 1f;
    public SoundType type;

    //[HideInInspector]
    [ReadOnly] [SerializeField] private AudioSource source;

    public AudioSource Source
    {
        get => source;
        set => source = value;
    }

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