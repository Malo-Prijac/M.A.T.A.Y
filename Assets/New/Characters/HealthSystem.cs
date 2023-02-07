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
    [SerializeField]private bool _isAlive = true;

    [SerializeField] private Canvas gameOver;
    [SerializeField] private Vector3 spawn;

    [Header("Sound Damaged")] 
    [SerializeField] private bool OverrideSoundDamaged;
    [ConditionalField("OverrideSoundDamaged")] [SerializeField] private Sound soundDamaged;
    
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
        //transform.position = _gameManager.currentSpawn.position;
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
        print("ok");

        if (OverrideSoundDamaged && soundDamaged.clip)
        {
            sound = soundDamaged;
        }
        
        if (sound!=null)
        {
            if(sound.clip)
                _audioManager.PlayClipAtPoint(sound);
        }
        
        currentHealth-=damage;
        
        if (currentHealth <= 0)
        {
            _isAlive = false;
        }

        if (!_isAlive)
        {
            if(gameObject.CompareTag("Player"))
            {
                reason = "Mort par "+reasonD;
                PlayerDeath();
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                GetComponent<CharacterControllerBase>().enabled = false;
                Destroy(gameObject,5);
            }
        }

        if (gameObject.CompareTag("Bush"))
        {
            _audioManager.DeleteSound(sound);
            transform.position += new Vector3(0, -200, 0);
            Destroy(gameObject,0.1f);
        }

        
    }
    
    public void PlayerDeath()
    {
        if (!gameOver)
            return;
        gameOver.enabled=true;
        transform.position = _gameManager.currentSpawn.position;
        gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reason;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void Respawn()
    {
        if (!_isAlive)
        {
            transform.position = _gameManager.currentSpawn.position;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameOver.enabled=false;
            currentHealth = maxHealth;
            _isAlive = true; 
        }
        
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
