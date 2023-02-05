using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float test = 0;
    public Transform forward;
    public TrapArrow ta;

    public bool hasUnlockedAttack;
    public bool dash;
    public bool shoot;
    public bool doubleJump;
    
    public int stateRingQuest = 0;
    public int orb = 0;
    public Vector3 currentSpawn;
    public Vector3 spawnWorld1;
    public Vector3 spawnWorld2;

    private Abilities abilities;
    public GameObject meleeWeapon; 


    public bool hasMeleeWeapon;
    // Start is called before the first frame update

    // Static singleton instance
    private static GameManager instance;
     
    // Static singleton property
    public static GameManager Instance
    {
        get => instance;
        set => instance = value;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)
 
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentSpawn = spawnWorld1;
        abilities = FindObjectOfType<Abilities>();
        //abilities.GiveMeleeWeaponToPlayer(meleeWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        test += Time.deltaTime;
        if (test >= 2f)
        {
            test = 0;
            //StartCoroutine(ta.ActivateTrap(0));
        }

        if (hasUnlockedAttack)
        {
            
        }

    }


    public void EquipPlayerMeleeWeapon()
    {
        hasMeleeWeapon = true;
        
    }
}
