using System.Collections;
using UnityEngine;

public class GiveDoubleJump : MonoBehaviour
{
    [SerializeField] private Canvas speakStatue;
    [SerializeField] private Canvas dialogue;
    [SerializeField] private Canvas tutoDJ;
    private bool printTuto = false;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && (other.CompareTag("Player")))
        {
            if (!_gameManager.doubleJump)
            {
                speakStatue.enabled = false;
                dialogue.enabled = true;
                _gameManager.doubleJump = true;
                printTuto = true;
                _gameManager.GiveDoubleJump();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_gameManager.doubleJump)
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
                StartCoroutine(TutoDoubleJump());
            }
        }
    }
    
    IEnumerator TutoDoubleJump()
    {
        tutoDJ.enabled = true;
        yield return new WaitForSeconds(3f);
        tutoDJ.enabled = false;
    }
}