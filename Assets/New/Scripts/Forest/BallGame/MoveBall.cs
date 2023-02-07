using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    public float speed;


 
    void FixedUpdate()
    {
        //SI la touche "fl�che du haut" est maintenue appuy�e
        if (Input.GetKey(KeyCode.Z))
        {
            //Ajout de force vers l'avant au rigidbody
            playerRigidbody.AddForce(Vector3.right * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerRigidbody.AddForce(Vector3.left * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            playerRigidbody.AddForce(Vector3.forward * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerRigidbody.AddForce(Vector3.back * Time.deltaTime * speed);
        }

    }

}
