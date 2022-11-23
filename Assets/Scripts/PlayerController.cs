using System.Collections;
using System.Collections.Generic;
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
        //On active un des deux objets au départ
        Object1.GetComponent<CharacterPlayerController>().enabled = true;
        cam1.SetActive(true);
        Object2.GetComponent<MoveBall>().enabled = false;
        cam2.SetActive(false);
        Object2.GetComponent<Timer>().enabled = false;

        choice = false;
    }

    // Update is called once per frame
    public void Change()
    {
        choice = !choice;
        if (choice)
        {
            Object1.GetComponent < CharacterPlayerController > ().enabled = false;
            cam1.SetActive(false);
            Object2.GetComponent < MoveBall > ().enabled = true;
            cam2.SetActive(true);
            Object2.GetComponent<Timer>().enabled = true;
        }
        else
        {
            Object1.GetComponent<CharacterPlayerController>().enabled = true;
            cam1.SetActive(true);
            Object2.GetComponent<MoveBall>().enabled = false;
            cam2.SetActive(false);
            Object2.GetComponent<Timer>().enabled = false;
        }
    }
}