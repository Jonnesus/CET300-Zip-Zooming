using UnityEngine;

public class PosCounter : MonoBehaviour
{
    public int carsPassed = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            carsPassed++;
    }
}