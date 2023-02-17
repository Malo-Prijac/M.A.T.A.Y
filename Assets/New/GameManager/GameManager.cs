using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum SceneToLoad
    {
    SCENE_HUB = 0,
    SCENE_FOREST = 1,
    SCENE_DESERT = 2
    }


    public bool hasUnlockedAttack;
    public bool hasUnlockedDash;
    public bool hasUnlockedRangedWeapon;
    public bool hasUnlockedDoubleJump;
    
    public int stateRingQuest = 0;
    public int relic = 0;
    public Transform currentSpawn;

    private Abilities abilities;
    public GameObject meleeWeapon;
    public GameObject rangeWeapon;
    private AudioManager _audioManager;
    [SerializeField]private Sound biomeSound;
    [SerializeField]private BiomeType biome = BiomeType.None;

    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject menuSound;
    
    [Header("Portails")]
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject portalHubDesert;
    public enum BiomeType
    {
        None,
        Desert,
        Forest
    }
    

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
        //Singleton method
        if (instance == null) {
            //First run, set the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
 
        } else if (instance != this) {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        hasUnlockedAttack = false;
        hasUnlockedDash = false;
        
        _audioManager = AudioManager.instance;
        abilities = FindObjectOfType<Abilities>();
        GetObjectives();
        abilities.GiveMeleeWeaponToPlayer(meleeWeapon);
        abilities.GiveRangedWeaponToPlayer(rangeWeapon);
        
    }

    private void Update()
    {
        UnlockPortail();
        if (Input.GetButtonDown("Pause"))
            ChangePauseState();
    }

    public void GiveMeleeWeapon()
    {
        hasUnlockedAttack = true;
        abilities._hasMeleeWeapon = true;
        abilities.SetObjectives();
    }
    
    public void GiveShoot()
    {
        hasUnlockedRangedWeapon = true;
        abilities._hasRangedWeapon = true;
        abilities.SetObjectives();
    }
    
    public void GiveDash()
    {
        hasUnlockedDash = true;
        abilities.GiveDash();
        abilities.SetObjectives();
    }
    
    public void GiveDoubleJump()
    {
        hasUnlockedDoubleJump = true;
        abilities.SetDoubleJump();
        abilities.SetObjectives();
    }

    public void ChangeBiome(BiomeType biomeToSet, Sound biomeSoundToSet)
    {
        Biome = biomeToSet;
        if(biomeSound != null)
            if (biomeSound.clip)
            {
                _audioManager.FadeOutSound(biomeSound);
            }

        if(biomeSoundToSet != null)
            if (biomeSoundToSet.clip)
            {
                _audioManager.FadeInSound(biomeSoundToSet);
            }
        
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
    
    public void GetObjectives()
    {
        stateRingQuest = PlayerPrefs.GetInt("stateRingQuest",0);
        relic = PlayerPrefs.GetInt("relic",0);
    }
    public void SetObjectives()
    {
        PlayerPrefs.SetInt("stateRingQuest",stateRingQuest);
        PlayerPrefs.SetInt("relic",relic);
    }

    public void UnlockPortail()
    {
        if (relic > 0)
        {
            if (portal)
            {
                portal.transform.GetChild(3).gameObject.SetActive(true);
                portal.transform.GetChild(4).gameObject.SetActive(true);
            }

            if (portalHubDesert)
            {
                portalHubDesert.transform.GetChild(3).gameObject.SetActive(true);
                portalHubDesert.transform.GetChild(4).gameObject.SetActive(true);
            }

        }
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene((int)SceneToLoad.SCENE_HUB);
        Time.timeScale = 1;
    }
}


