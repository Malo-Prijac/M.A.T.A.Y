using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time = 10;
    public GameObject afficheTimer;
    private bool timeRunning = true;

    void Start()
    {
        afficheTimer.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        float seconds = Mathf.FloorToInt(time % 60);
        float centiseconds = Mathf.FloorToInt((time * 100.0f) % 100);
        afficheTimer.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", seconds, centiseconds);


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
            }
        }
        

    }

}
