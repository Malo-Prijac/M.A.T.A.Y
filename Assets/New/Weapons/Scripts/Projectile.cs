using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float destroyTime;
    private Rigidbody _rb;
    private BoxCollider _collider;
    private GameManager _gameManager;
    private GameObject _owner;
    private string targetTag;

    public string TargetTag
    {
        get => targetTag;
        set => targetTag = value;
    }

    public GameObject Owner
    {
        get => _owner;
        set => _owner = value;
    }
    public float Damage
    {
        get => damage;
        set => damage = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        Destroy(gameObject, destroyTime);
        _gameManager = GameManager.Instance;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            HealthSystem playerHealthSystem = collision.gameObject.GetComponent<HealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(damage,"le projectile d\'un ennemi");
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        /*
        _rb.velocity = Vector3.zero;
        Destroy (_rb);
        Destroy (_collider);
        transform.parent = other.gameObject.transform;
        if (other.gameObject.CompareTag("Player"))
        {

            HealthSystem playerHealthSystem = other.gameObject.GetComponent<HealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(damage,"le projectile d\'un ennemi");
            //Destroy(gameObject);
        }
        */
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem playerHealthSystem = other.gameObject.GetComponent<HealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(damage,"le projectile d\'un ennemi");
            
            //print("ok");
            //_gameManager.playerHealthSystem.TakeDamage(damage,"le projectile d\'un ennemi");
            //Destroy(gameObject);
        }
        
    }
}
