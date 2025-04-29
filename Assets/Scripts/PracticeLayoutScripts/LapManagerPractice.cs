using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class LapManagerPractice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentTimeText, bestTimeText, lapCountText;
    [SerializeField] private int maximumLaps = 999999;

    private int minuteCount, secondCount, lapNumber = 1;
    private float milliCount, hiddenCurrentTime, hiddenBestTime;
    private bool firstLap = true;
    private string minutes, seconds;

    private void Start()
    {
        bestTimeText.text = "Best: --:--.--";
        lapCountText.text = "Lap: 1";
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        milliCount += Time.deltaTime * 10;
        hiddenCurrentTime += Time.deltaTime;

        if (milliCount >= 10)
        {
            milliCount = 0;
            secondCount += 1;
        }

        if (secondCount <= 9)
            seconds = "0" + Convert.ToString(secondCount);
        else
            seconds = "" + Convert.ToString(secondCount);

        if (secondCount >= 60)
        {
            secondCount = 0;
            minuteCount += 1;
        }

        if (minuteCount <= 9)
            minutes = "0" + Convert.ToString(minuteCount);
        else
            minutes = "" + Convert.ToString(minuteCount);

        currentTimeText.text = "Time: " + minutes + ":" + seconds + "." + milliCount.ToString("F0");
    }

    public void LapCounter()
    {
        // Set best lap time
        if (firstLap)
        {
            bestTimeText.text = "Best: " + minutes + ":" + seconds + "." + milliCount.ToString("F0");
            hiddenBestTime = hiddenCurrentTime;
            hiddenCurrentTime = 0;
            firstLap = false;
        }
        else if (hiddenCurrentTime < hiddenBestTime)
        {
            bestTimeText.text = "Best: " + minutes + ":" + seconds + "." + milliCount.ToString("F0");
            hiddenBestTime = hiddenCurrentTime;
            hiddenCurrentTime = 0;
        }
        else
            hiddenCurrentTime = 0;

        milliCount = 0;
        secondCount = 0;
        minuteCount = 0;
        lapNumber++;

        lapCountText.text = "Lap: " + lapNumber;
    }
}