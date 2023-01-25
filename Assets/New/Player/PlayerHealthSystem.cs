using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthSystem : MonoBehaviour
{
    
    [SerializeField] private float health = 100;
    public GameObject dead;
    private string reason = "";
    //public GameObject reasonDeath;
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
                Death();
            }
           
        }
    }
    
    public void Death()
    {
        GameObject GameOver = Instantiate(dead, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        GameOver.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);
        GameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reason;
        GameOver.SetActive(true);
    }
}
