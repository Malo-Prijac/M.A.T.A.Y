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
        if (Input.GetKey(KeyCode.O) && (other.CompareTag("Player")))
        {
            if (!_gameManager.dash)
            {
                speakStatue.enabled = false;
                dialogue.enabled = true;
                _gameManager.dash = true;
                printTuto = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_gameManager.dash)
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