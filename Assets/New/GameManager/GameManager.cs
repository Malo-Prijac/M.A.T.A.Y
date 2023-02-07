using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool hasUnlockedAttack;
    public bool dash;
    public bool shoot;
    public bool doubleJump;
    
    public int stateRingQuest = 0;
    public int relic = 0;
    public Transform currentSpawn;

    private Abilities abilities;
    public GameObject meleeWeapon;
    public GameObject rangeWeapon;
    private AudioManager _audioManager;
    private Sound biomeSound;

    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject menuSound;
    public enum BiomeType
    {
        None,
        Desert,
        Forest
    }
    
    private BiomeType biome = BiomeType.None;

    public BiomeType Biome
    {
        get => biome;
        set => biome = value;
    }

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
        _audioManager = AudioManager.instance;
        abilities = FindObjectOfType<Abilities>();
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
            ChangePauseState();
    }

    public void GiveWeapon()
    {
        if (hasUnlockedAttack)
        {
            abilities.GiveMeleeWeaponToPlayer(meleeWeapon);
        }
    }
    
    public void GiveShoot()
    {
        if (shoot)
        {
            abilities.GiveRangedWeaponToPlayer(rangeWeapon);
        }
    }
    
    public void GiveDash()
    {
        if (dash)
        {
            abilities.GiveDash();
        }
    }
    
    public void GiveDoubleJump()
    {
        if (doubleJump)
        {
            abilities.AddJump();
        }
    }

    public void ChangeBiome(BiomeType biomeToSet, Sound biomeSoundToSet)
    {
        Biome = biomeToSet;
        if(biomeSound!= null)
            if(biomeSound.clip)
                _audioManager.FadeOutSound(biomeSound);
                //_audioManager.DeleteSound(biomeSound);
        
        if(biomeSoundToSet!= null)
            if(biomeSoundToSet.clip)
                _audioManager.FadeInSound(biomeSoundToSet);
                //_audioManager.Play(biomeSoundToSet);
        
        biomeSound = biomeSoundToSet;
    }

    public void ChangePauseState()
    {
        Time.timeScale = 1-Time.timeScale;
        if (!menuSound.activeSelf)
        {
            menuPause.SetActive(!menuPause.activeSelf);
            if (menuPause)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            menuSound.SetActive(false);
            menuPause.SetActive(!menuPause.activeSelf);
        }
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}


