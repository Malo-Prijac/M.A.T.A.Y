using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testdem : MonoBehaviour
{
    private Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i])
                return;
            if (colliders[i].isTrigger)
            {
                Transform[] t = colliders[i].GetComponentsInChildren<Transform>();
                for (int j = 0; j < colliders.Length; j++)
                {
                    Destroy(t[j].gameObject);

                }
            }
        }

    }
    
}