using System.Collections;
using UnityEngine;

public class GiveDash : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas tutoDash;
    private bool printTuto = false;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if(PlayerPrefs.GetInt("hasDash",0) == 1)
            return;
        if (Input.GetKey(KeyCode.F) && (other.CompareTag("Player")))
        {
            if (!_gameManager.hasUnlockedDash)
            {
                speakStatue.enabled = false;
                dialogue.enabled = true;
                printTuto = true;
                _gameManager.GiveDash();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(PlayerPrefs.GetInt("hasDash",0) == 1)
            return;
        if (other.CompareTag("Player"))
        {
            if (!_gameManager.hasUnlockedDash)
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
                StartCoroutine(TutoDash());
            }
        }
    }
    
    IEnumerator TutoDash()
    {
        tutoDash.enabled = true;
        yield return new WaitForSeconds(3f);
        tutoDash.enabled = false;
    }
}