using UnityEngine;

public class AdaptiveDifficulty : MonoBehaviour
{
    public float currentDifficulty;

    [SerializeField] private LapManager lapManager;

    private const int maximumDifficulty = 100, minimumDifficulty = 1;
    private int startingPos = 10, endingPos, posDifference;

    private void Awake()
    {
        AIDifficulty aiDifficulty = SaveSystem.LoadDifficulty();

        currentDifficulty = aiDifficulty.difficulty;

        if (currentDifficulty <= 0 || currentDifficulty > 100)
            currentDifficulty = 50;
    }

    public void CalculateDifficulty()
    {
        endingPos = lapManager.posCounter;

        if (endingPos == 10)
            currentDifficulty += 15;
        else if (endingPos > 10 && endingPos <= 13)
            currentDifficulty += 10;
        else if (endingPos > 13 && endingPos <= 15)
            currentDifficulty += 5;
        else if (endingPos > 15 && endingPos <= 17)
            currentDifficulty -= 5;
        else if (endingPos > 17  && endingPos < 20)
            currentDifficulty -= 10;
        else
            currentDifficulty -= 15;

        posDifference = startingPos - endingPos;
        currentDifficulty += posDifference;

        if (currentDifficulty >= maximumDifficulty)
        {
            currentDifficulty = maximumDifficulty;
        }
        else if (currentDifficulty <= minimumDifficulty)
        {
            currentDifficulty = minimumDifficulty;
        }
    }
}