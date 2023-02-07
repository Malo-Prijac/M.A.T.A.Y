using System.Collections;
using UnityEngine;

public class GiveShoot : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas tutoShoot;
    private bool printTuto = false;
    
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(PlayerPrefs.GetInt("_hasRangedWeapon",0) == 1)
            return;
        if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
        {
            if (!_gameManager.shoot)
            {
                speakStatue.enabled = false;
                dialogue.enabled = true;
                _gameManager.shoot = true;
                printTuto = true;
                _gameManager.GiveShoot();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(PlayerPrefs.GetInt("_hasRangedWeapon",0) == 1)
            return;
        if (other.CompareTag("Player"))
        {
            if (!_gameManager.shoot)
            {
                speakStatue.enabled=true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speakStatue.enabled = false;
            dialogue.enabled = false;
            if (printTuto)
            {
                printTuto = false;
                StartCoroutine(TutoShoot());
            }
        }
    }
    
    IEnumerator TutoShoot()
    {
        tutoShoot.enabled = true;
        yield return new WaitForSeconds(3f);
        tutoShoot.enabled = false;
    }
}