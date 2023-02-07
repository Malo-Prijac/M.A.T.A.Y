using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    public float speed;


 
    void Update()
    {
        //SI la touche "fl�che du haut" est maintenue appuy�e
        if (Input.GetKey(KeyCode.Z))
        {
            //Ajout de force vers l'avant au rigidbody
            playerRigidbody.AddForce(Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerRigidbody.AddForce(Vector3.left * speed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            playerRigidbody.AddForce(Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerRigidbody.AddForce(Vector3.back * speed);
        }

    }

}
