using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaHand : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas dialogue2;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Canvas tutoFight;

    private bool questFinished = false;
    private GameManager _gameManager;
    // Start is called before the first frame update

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerPrefs.GetInt("_hasMeleeWeapon", 0) == 1)
            return;
        if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
        {
            if (!questFinished)
            {
                speakStatue.enabled = false;
                _gameManager = GameManager.Instance;
                if (_gameManager.stateRingQuest==2)
                {
                    questFinished = true;
                    StartCoroutine(TutoFight());
                }
                else
                {
                    _gameManager.stateRingQuest = 1;
                    dialogue.enabled = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerPrefs.GetInt("_hasMeleeWeapon", 0) == 1)
            return;
        
        if (!questFinished)
        {
            if (other.CompareTag("Player"))
            {
                speakStatue.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakStatue.enabled = false;
            dialogue.enabled = false;
        }
    }
    
    IEnumerator TutoFight()
    {
        
        dialogue2.enabled=true;
        yield return new WaitForSeconds(3f);
        dialogue2.enabled = false;
        _gameManager.GiveMeleeWeapon();
        tutoFight.enabled = true;
        enemy.SetActive(true);
        yield return new WaitForSeconds(3f);
        tutoFight.enabled = false;
    }
}
