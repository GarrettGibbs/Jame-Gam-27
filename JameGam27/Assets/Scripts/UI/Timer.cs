using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timerText;
    private float startingTime = 90f;
    private float currentTime;
    private bool timerActive;
    [SerializeField] GameManager gameManager;
    private bool timerSoundStarted = false;
    private bool spawned1Squirrels = false;
    private bool spawned2Squirrels = false;

    public void StartTimer() {
        currentTime = startingTime;
        timerActive = true;
    }

    void Update() {
        if (timerActive) {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f) {
                currentTime = 0f;
                timerActive = false;
                TimerFinished();
            } else {
                DisplayTime(currentTime);
            }
            if (currentTime <= 6f && !timerSoundStarted) {
                gameManager.audioManager.PlayCountdown(true); 
                timerSoundStarted = true;
                timerText.color = Color.red;
                //timerText.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            } else if(currentTime <= 60f && !spawned1Squirrels) {
                spawned1Squirrels = true;
                gameManager._gridManager.SpawnSpquirrel(1);
                gameManager._gridManager.SpawnSpquirrel(2);
            } else if(currentTime <= 30f && !spawned2Squirrels) {
                spawned2Squirrels = true;
                gameManager._gridManager.SpawnSpquirrel(1);
                gameManager._gridManager.SpawnSpquirrel(2);
            }
        }
    }

    void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void TimerFinished() {
        timerText.text = "0:00";
        gameManager.EndGame();
    }
}
