using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [ReadOnly][SerializeField] private float currentHealth;
    [SerializeField] private Image healthBar;
    private bool alive = true;
    private string reason = "";
    [SerializeField] private Canvas gameOver;
    [SerializeField] private Vector3 spawn;

    [Header("Sound Damaged")] 
    [SerializeField] private bool OverrideSoundDamaged;
    [ConditionalField("OverrideSoundDamaged")] [SerializeField] private Sound soundDamaged;
    
    private AudioManager _audioManager;
    private GameManager _gameManager;
    void Start()
    {
        _gameManager = GameManager.Instance;
        currentHealth = maxHealth;
        _audioManager = AudioManager.instance;
        _audioManager.AddNewSound(soundDamaged, gameObject);

        //audioManager.AddNewSound(damageSound);
    }
    
    void Update()
    {
        if (healthBar)
        {
            healthBar.fillAmount = currentHealth/maxHealth;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Heal();
        }
    }
    

    public void Heal()
    {
        if (currentHealth >= 50)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += 50;
        }
    }
    
    public void TakeDamage(float damage, string reasonD, Sound sound = null)
    {
        if (!OverrideSoundDamaged && sound != null)
        {
            soundDamaged = sound;
        }
        
        if (soundDamaged.clip)
        {
            _audioManager.Play(soundDamaged);
        }
        //print("PLAYER TAKE " + damage);
        currentHealth-=damage;
        
        if (gameObject.CompareTag("Player"))
        {
            if (currentHealth <= 0)
            {
                if (alive)
                {
                    alive = false;
                    reason = "Mort par "+reasonD;
                    PlayerDeath();
                }
           
            }
        }
    }
    
    public void PlayerDeath()
    {
        if (!gameOver)
            return;
        gameOver.enabled=true;
        transform.position = spawn;
        gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reason;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void Respawn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOver.enabled=false;
        currentHealth = maxHealth;
        alive = true;
    }
}
