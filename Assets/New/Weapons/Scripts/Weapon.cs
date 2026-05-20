using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField][ReadOnly]protected GameObject owner;
    [SerializeField] protected Sound attackSound;
    protected AudioManager _audioManager;
    protected string targetTag;
    protected bool isAttacking;

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }
    public string TargetTag
    {
        get => targetTag;
        set => targetTag = value;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        _audioManager = AudioManager.instance;
        if (attackSound.clip)
        {
            _audioManager.AddNewSound(attackSound, gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject Owner
    {
        get => owner;
        set => owner = value;
    }
    public virtual void Attack(Vector3 targetPosition = default(Vector3), float delaySoundAttack = 0, Vector3 startPosition = default(Vector3))
    {
        if (attackSound.clip)
        {
            _audioManager.Play(attackSound,delaySoundAttack);
        }
    }
    
}

