using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTextDialogue : MonoBehaviour
{
    [SerializeField] private GameObject pressO;
    [SerializeField] private GameObject dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            pressO.SetActive(false);
            dialogue.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pressO.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        pressO.SetActive(false);
        dialogue.SetActive(false);
    }
}
