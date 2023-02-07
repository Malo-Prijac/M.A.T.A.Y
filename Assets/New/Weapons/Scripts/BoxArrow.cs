using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxArrow : MonoBehaviour
{
    private RangedWeapon _weapon;
    [SerializeField] private Transform start;

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
        _weapon.TargetTag = "Player";
        _weapon.Attack(start.position+start.right, delaySound, start.position);
    }
}
