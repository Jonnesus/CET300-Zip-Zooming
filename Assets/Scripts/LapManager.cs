using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using OmniVehicleAi;
using ArcadeVP;

public class LapManager : MonoBehaviour
{
    public int posCounter;
    [SerializeField] private AdaptiveDifficulty adaptiveDifficulty;
    [SerializeField] private TextMeshProUGUI currentTimeText, bestTimeText, lapCountText, positionReadoutText, countdownText;
    [SerializeField] private GameObject resultsPanel, restartButton, countdownTextObject;
    [SerializeField] private GameObject[] aiCars, checkpointPositionCollision;
    [SerializeField] private int maximumLaps = 3;

    private int minuteCount, secondCount, lapNumber = 1;
    private float milliCount, hiddenCurrentTime, hiddenBestTime;
    private bool firstLap = true, raceStarted = false;
    private string minutes, seconds;

    private void Start()
    {
        bestTimeText.text = "Best: --:--.--";
        lapCountText.text = "Lap: 1/" + maximumLaps;
        StartCoroutine(CountdownTimer());
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (raceStarted)
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

        // End race + show results
        if (lapNumber > maximumLaps)
        {
            // End race here
            lapNumber = maximumLaps;
            lapCountText.text = "Lap: " + lapNumber + "/" + maximumLaps;

            positionReadoutText.text = posCounter switch
            {
                10 => "You finished 1st!",
                11 => "You finished 2nd!",
                12 => "You finished 3rd!",
                13 => "You finished 4th!",
                14 => "You finished 5th!",
                15 => "You finished 6th!",
                16 => "You finished 7th!",
                17 => "You finished 8th!",
                18 => "You finished 9th!",
                19 => "You finished 10th!",
                _ => "You finished!"
            };

            adaptiveDifficulty.CalculateDifficulty();

            GameObject.Find("Player Car").GetComponent<InputManager_Player>().enabled = false;
            resultsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(restartButton);
        }
        else
        {
            foreach (GameObject gameObject in checkpointPositionCollision)
            {
                gameObject.GetComponent<CheckpointPositionCollision>().carsPassed = 0;
            }

            lapCountText.text = "Lap: " + lapNumber + "/" + maximumLaps;
        }
    }

    IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(1f);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Go!";
        raceStarted = true;

        GameObject.Find("Player Car").GetComponent<InputManager_Player>().enabled = true;

        foreach (GameObject gameObject in aiCars)
        {
            gameObject.GetComponent<ArcadeVP_inputProvider>().enabled = true;
        }

        yield return new WaitForSeconds(1f);
        countdownTextObject.SetActive(false);
    }
}