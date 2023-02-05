using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGiveWeapon : MonoBehaviour
{
    public GameObject meleeWeapon;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            FindObjectOfType<Abilities>().GiveMeleeWeaponToPlayer(meleeWeapon);
            print("ui");
        }
    }
}
