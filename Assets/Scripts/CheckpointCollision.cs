using UnityEngine;

public class CheckpointCollision : MonoBehaviour
{
    [SerializeField] private LapManager lapManager;

    private bool cp1 = false, cp2 = false, cp3 = false, cp4 = false, cp5 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Checkpoint 1")
            cp1 = true;
        else if (other.gameObject.name == "Checkpoint 2")
            cp2 = true;
        else if (other.gameObject.name == "Checkpoint 3")
            cp3 = true;
        else if (other.gameObject.name == "Checkpoint 4")
            cp4 = true;
        else if (other.gameObject.name == "Checkpoint 5")
            cp5 = true;
        else if (other.gameObject.name == "Start Finish" && cp1 && cp2 && cp3 && cp4 && cp5)
        {
            cp1 = false;
            cp2 = false;
            cp3 = false;
            cp4 = false;
            cp5 = false;
            lapManager.LapCounter();
        }
    }
}