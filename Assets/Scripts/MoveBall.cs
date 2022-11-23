using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    //Référence au rigidbody de la bille
    public Rigidbody playerRigidbody;

    //Vitesse à assigner via l'inspecteur
    public float speed;



    // Update is called once per frame
    void Update()
    {
        //SI la touche "flèche du haut" est maintenue appuyée
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Ajout de force vers l'avant au rigidbody
            playerRigidbody.AddForce(Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerRigidbody.AddForce(Vector3.back * speed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRigidbody.AddForce(Vector3.left * speed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRigidbody.AddForce(Vector3.right * speed);
        }

    }

}
