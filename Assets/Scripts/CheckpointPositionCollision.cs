using UnityEngine;

public class CheckpointPositionCollision : MonoBehaviour
{
    public int carsPassed;

    [SerializeField] private LapManager lapManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == ("Player Car"))
        {
            carsPassed++;
            lapManager.posCounter = carsPassed;
        }
        else if (other.CompareTag("Player"))
        {
            carsPassed++;
        }
    }
}