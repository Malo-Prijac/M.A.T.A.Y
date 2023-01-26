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
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    void Update()
    {
        healthBar.fillAmount = currentHealth/maxHealth;
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
        //this.GetComponent<PlayerCharacterController>().enabled = false;
        gameOver.SetActive(true);
        gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reason;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = new Vector3(36.8699989f,5.44000006f,-8.89000034f);
        Debug.Log("Yo");
    }
    
    public void Respawn()
    {
        Debug.Log("COUCOU");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //this.GetComponent<PlayerCharacterController>().enabled = true;
        gameOver.SetActive(false);
        currentHealth = 100;
        alive = true;
    }
}
