using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Object1;
    public GameObject Object2;
    public GameObject cam1;
    public GameObject cam2;
    private bool choice;

    void Start()
    {
        choice = false;
    }

    public void Change()
    {
        choice = !choice;
        if (choice)
        {
            Object1.GetComponent<PlayerCharacterController>().enabled = false;
            cam1.SetActive(false);
            Object2.GetComponent<MoveBall> ().enabled = true;
            cam2.SetActive(true);
            Object2.GetComponent<Timer>().enabled = true;
            Object2.GetComponent<Timer>().Init();
        }
        else
        {
            Object1.GetComponent<PlayerCharacterController>().enabled = true;
            cam1.SetActive(true);
            Object2.GetComponent<MoveBall>().enabled = false;
            cam2.SetActive(false);
            Object2.GetComponent<Timer>().enabled = false;
        }
    }
}