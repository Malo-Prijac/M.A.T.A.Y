using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishParcours : MonoBehaviour
{

    public PlayerController playerController;
    [SerializeField] private List<GameObject> _linkedDoors;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in _linkedDoors)
        {
            go.GetComponent<Animator>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameObject.Find("Ball").GetComponent<Timer>().time > 0)
            {
                playerController.Change();
                foreach (GameObject go in _linkedDoors)
                {
                    go.GetComponent<Animator>().enabled = true;
                }
                GameObject.Find("Ball").GetComponent<Timer>().afficheTimer.SetActive(false);
                Destroy(GameObject.Find("MiniJeuBoule"));
                Destroy(GameObject.Find("Ball"));
            } 
        }
    }
}
