
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the drone enters the finish zone
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Tell GameManager the drone reached the finish
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerWin();
        }
    }
}
 