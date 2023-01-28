using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float destroyTime;
    private Rigidbody _rb;
    private BoxCollider _collider;
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
        StartCoroutine(DestroyAfterTime());

    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        if(gameObject)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        /*
        transform.parent = other.gameObject.transform;
        _rb.velocity = Vector3.zero;
        Destroy (_rb);
        Destroy (_collider);
        */
        if (other.gameObject.CompareTag("Player"))
        {

            PlayerHealthSystem playerHealthSystem = other.gameObject.GetComponent<PlayerHealthSystem>();
            if (!playerHealthSystem)
                return;
            playerHealthSystem.TakeDamage(damage,"le projectile d\'un ennemi");
            
        }
        Destroy(gameObject);
        
    }
}
