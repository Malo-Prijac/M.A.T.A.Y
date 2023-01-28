using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DallePiege : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        /*
        transform.parent = other.gameObject.transform;
        _rb.velocity = Vector3.zero;
        Destroy (_rb);
        Destroy (_collider);
        */
        Debug.Log("TO");
        if (other.gameObject.CompareTag("Player"))
        {
            HealthSystem playerHealthSystem = other.gameObject.GetComponent<HealthSystem>();
            //gameObject.SetActive(false);
            //gameObject.SetActive(true);
            StartCoroutine(Fall(playerHealthSystem));
        }
    }
    
    IEnumerator Fall(HealthSystem playerHealthSystem)
    {
        Debug.Log("TO2");
        yield return new WaitForSeconds(2f);
        Debug.Log("TO222");
        playerHealthSystem.TakeDamage(100,"chute vertigineuse");
    }
}
