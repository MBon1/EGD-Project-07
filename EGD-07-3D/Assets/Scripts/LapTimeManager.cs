using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour
{
    private float timeElapsed = 0f;
    private bool isRunning = false;

    private float bestTime;

    public Text lapTimeText;
    public Text bestTimeText;
    public GameObject newBestTime;

    public bool startStopWatch;
    public bool displayResults;

    // Start is called before the first frame update
    void Start()
    {
        newBestTime.SetActive(false);
        if (PlayerPrefs.HasKey("bestTime"))
        {
            bestTime = PlayerPrefs.GetFloat("bestTime");
            bestTimeText.text = GetBestTime();
        }
        else
        {
            bestTime = -1;
            bestTimeText.text = "--:--.---";
        }

        if (displayResults)
        {
            DisplayResults();
        }
        else
        {
            if (startStopWatch)
                StartStopwatch();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
            lapTimeText.text = GetTimeElapsed();
        }
    }

    public void StartStopwatch()
    {
        timeElapsed = 0f;
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
        PlayerPrefs.SetFloat("timeElapsed", timeElapsed);
    }

    public string GetTimeElapsed()
    {
        return FormatTime(timeElapsed);
    }
    public string GetBestTime()
    {
        return FormatTime(bestTime);
    }

    public string FormatTime(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void DisplayResults()
    {
        if (PlayerPrefs.HasKey("timeElapsed"))
        {
            timeElapsed = PlayerPrefs.GetFloat("timeElapsed");
            lapTimeText.text = GetTimeElapsed();

            if (timeElapsed < bestTime || bestTime < 0)
            {
                PlayerPrefs.SetFloat("bestTime", timeElapsed);
                bestTime = timeElapsed;
                // NEW BEST TIME!
                newBestTime.SetActive(true);
                bestTimeText.text = GetBestTime();
            }
        }
        else
        {
            timeElapsed = Mathf.Infinity;
            lapTimeText.text = "--:--.---";
        }

    }
}
