using UnityEngine;

// Bendera di ujung level: sentuh = level selesai.
public class GoalFlag : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        if (GameManager.Instance != null) GameManager.Instance.CompleteLevel();
    }
}
