using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Player Tag")]
    [ReadOnly] [SerializeField] protected string playerTag ="Player";

    [Header("Owner")]
    [SerializeField][ReadOnly]protected GameObject owner;

    protected GameObject _player;
    protected float _playerheight;

    
    // Start is called before the first frame update
    public void Start()
    {
        _player = GameObject.FindWithTag(playerTag);
        _playerheight = _player.GetComponent<CapsuleCollider>().height;
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
    public virtual void Attack()
    {
    }
    
}

