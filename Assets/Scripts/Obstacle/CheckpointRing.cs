
using UnityEngine;

public class CheckpointRing : MonoBehaviour
{
    private bool isCleared = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCleared) return;

        if (other.CompareTag("Player"))
        {
            isCleared = true;

            Debug.Log("Entered Checkpoint");
        }
    }
}
