using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;

    public float currentHealth;

    public TextMeshProUGUI _displayHealth;

    public Image HealthBar;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        _displayHealth.text = currentHealth + "/100";
        HealthBar.fillAmount = currentHealth/maxHealth;
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal();
        }
    }

    public void TakeDamage(float damage, string t)
    {
        currentHealth-=damage;
        
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("Dead");
        currentHealth = maxHealth;
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
}
