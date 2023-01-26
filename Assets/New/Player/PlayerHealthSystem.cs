using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private Image healthBar;
    private bool alive = true;
    private string reason = "";
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Vector3 spawn;
    
    void Start()
    {
        currentHealth = maxHealth;
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
    
    public void TakeDamage(float damage, string reasonD)
    {
        print("PLAYER TAKE " + damage);
        currentHealth-=damage;
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
    
    public void PlayerDeath()
    {
        gameOver.SetActive(true);
        transform.position = spawn;
        gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reason;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    public void Respawn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOver.SetActive(false);
        currentHealth = maxHealth;
        alive = true;
    }
}
