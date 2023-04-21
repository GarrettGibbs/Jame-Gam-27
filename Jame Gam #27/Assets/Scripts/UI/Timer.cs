using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float startingTime = 60f;
    private float currentTime;
    private bool timerActive;

    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        currentTime = startingTime;
        timerActive = true;
    }

    void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                timerActive = false;
                TimerFinished();
            }
            else
            {
                DisplayTime(currentTime);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        timerText.text = "0:00";
        Debug.Log("Timer has finished");
    }
}
