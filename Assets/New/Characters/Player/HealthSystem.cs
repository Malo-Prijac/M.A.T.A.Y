using System.Collections;
using System.Collections.Generic;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Animator")]
    [ReadOnly][SerializeField] private Animator animator;
    private static readonly int IsAlive = Animator.StringToHash("IsAlive");

    [Header("Health Stats")]
    [SerializeField] private float maxHealth = 100f;
    [ReadOnly][SerializeField] private float currentHealth;
    [SerializeField] private Image healthBar;

    [SerializeField] private Canvas gameOver;
    [SerializeField] private Vector3 spawn;

    [Header("Sound Damaged")] 
    [SerializeField] private bool OverrideSoundDamaged;
    [ConditionalField("OverrideSoundDamaged")] [SerializeField] private Sound soundDamaged;
    
    [ReadOnly][SerializeField]private bool _isAlive = true;
    private string reason = "";
    private AudioManager _audioManager;
    private GameManager _gameManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        _gameManager = GameManager.Instance;
        currentHealth = maxHealth;
        _audioManager = AudioManager.instance;
        _audioManager.AddNewSound(soundDamaged, gameObject);
        _isAlive = true;
    }
    
    void Update()
    {
        AnimationBehavior();
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
        //if (!_isAlive)
            //return;
        
        _audioManager.Play(soundDamaged);
        if (OverrideSoundDamaged && soundDamaged.clip)
        {
            //print("SON");
            _audioManager.Play(soundDamaged,soundDamaged.clip.length);
        }
        else if (!OverrideSoundDamaged && sound!=null)
        {
            _audioManager.Play(sound,sound.clip.length);
        }
        
        currentHealth-=damage;
        
        if (gameObject.CompareTag("Player"))
        {

            if (_isAlive)
            {
                reason = "Mort par "+reasonD;
                PlayerDeath();
            }
        
        }

        if (gameObject.CompareTag("Bush"))
        {
            gameObject.SetActive(false);
        }

        /*if (currentHealth <= 0)
        {
            _isAlive = false;
            GetComponent<CharacterControllerBase>().enabled = false;
        }*/
    }
    
    public void PlayerDeath()
    {
        if (!gameOver)
            return;
        gameOver.enabled=true;
        transform.position = _gameManager.currentSpawn;
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
        _isAlive = true;
    }
    
    private void AnimationBehavior()
    {
        if (!animator)
        {
            Debug.LogWarning("No Animator Character on "+name);
            return;  
        }

        animator.SetBool(IsAlive, _isAlive);

    }
}
