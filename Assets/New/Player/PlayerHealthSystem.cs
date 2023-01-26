using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;

    public float currentHealth;
    
    public Image HealthBar;

    private bool alive = true;

    public string reason = "";

    public GameObject gameOver;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        HealthBar.fillAmount = currentHealth/maxHealth;
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
        this.GetComponent<PlayerCharacterController>().enabled = false;
        gameOver.SetActive(true);
        gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reason;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = new Vector3(0, 0, 0);
    }
    
    public void Respawn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.GetComponent<PlayerCharacterController>().enabled = true;
        gameOver.SetActive(false);
        currentHealth = 100;
        alive = true;
    }
}
