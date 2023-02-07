using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPath : MonoBehaviour
{

    [SerializeField] private Sound soundDoors;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<GameObject> _linkedDoors;
    [SerializeField] private GameObject Ball;

    private AudioManager _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = AudioManager.instance;
        foreach (GameObject go in _linkedDoors)
        {
            go.GetComponent<Animator>().enabled = false;
            
        }
        
        if(soundDoors != null)
            if(soundDoors.clip)
                _audioManager.AddNewSound(soundDoors,_linkedDoors[0]);
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (Ball.GetComponent<Timer>().time > 0)
            {
                playerController.Change();
                foreach (GameObject go in _linkedDoors)
                {
                    go.GetComponent<Animator>().enabled = true;
                }
                Ball.GetComponent<Timer>().printTimer.SetActive(false);
                Ball.GetComponent<Timer>().enabled = false;

                if(soundDoors != null)
                    if (soundDoors.Source)
                        _audioManager.Play(soundDoors);
            } 
        }
    }
}
