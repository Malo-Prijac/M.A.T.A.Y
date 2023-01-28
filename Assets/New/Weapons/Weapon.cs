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
    
    protected AudioManager audioManager;
    // Start is called before the first frame update
    protected void Start()
    {
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
    public virtual void Attack(Vector3 targetPosition)
    {
    }
    
}

