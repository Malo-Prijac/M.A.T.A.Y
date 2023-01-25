using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthSystem : MonoBehaviour
{
    
    [SerializeField] private float health = 100;
    [SerializeField] private GameObject gameOver;
    private string reason = "";
    private bool alive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void TakeDamage(float damage, string reasonD)
    {
        print("PLAYER TAKE " + damage);
        health = health - damage;
        if (health <= 0)
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
    }
    
    public void Respawn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.GetComponent<PlayerCharacterController>().enabled = true;
        gameOver.SetActive(false);
        transform.position = new Vector3(0, 0, 0);
    }
}
