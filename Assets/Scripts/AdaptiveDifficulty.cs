using UnityEngine;

public class AdaptiveDifficulty : MonoBehaviour
{
    public float currentDifficulty;

    private const int maximumDifficulty = 100, minimumDifficulty = 1;
    private int startingPos, endingPos, posDifference;

    public void CalculateDifficulty()
    {
        if (endingPos == 1)
            currentDifficulty += 15;
        else if (endingPos > 1 && endingPos <= 3)
            currentDifficulty += 10;
        else if (endingPos > 3 && endingPos <= 5)
            currentDifficulty += 5;
        else if (endingPos > 5 && endingPos <= 7)
            currentDifficulty -= 5;
        else if (endingPos > 7  && endingPos < 10)
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