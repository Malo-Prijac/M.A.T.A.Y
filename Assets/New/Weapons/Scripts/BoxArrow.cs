using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxArrow : MonoBehaviour
{
    private RangedWeapon _weapon;
    [SerializeField] private Transform forward;
    [SerializeField] private float delaySound;
    // Start is called before the first frame update
    void Start()
    {
        _weapon = GetComponentInChildren<RangedWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        _weapon.Attack(forward.position, delaySound);
    }
}
