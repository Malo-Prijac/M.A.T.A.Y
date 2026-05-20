using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeMax = 10;
    public float time = 10;
    public GameObject printTimer;
    private bool timeRunning = true;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BallGame ballGame;
    

    public void Init()
    {
        transform.position = new Vector3(-65, 2.44499993f, 100);
        timeRunning = true;
        time = timeMax;
        printTimer.SetActive(true);
    }
    void Update()
    {
        
        float seconds = Mathf.FloorToInt(time % 60);
        float centiseconds = Mathf.FloorToInt((time * 100.0f) % 100);
        printTimer.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", seconds, centiseconds);


        if (timeRunning)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            if (time <= 0)
            {
                time = 0;
                timeRunning = false;
                
                ballGame.GetComponent<BallGame>()._isOn = false;
                printTimer.SetActive(false);
                playerController.Change();
            }
        }

    }

}
