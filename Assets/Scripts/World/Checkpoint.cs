using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer flag;
    bool active;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (active || other.GetComponent<PlayerController>() == null) return;
        active = true;
        if (flag != null) flag.color = new Color(0.1f, 0.71f, 0.65f);
        if (GameManager.Instance != null) GameManager.Instance.Checkpoint = new Vector2(transform.position.x, 2.6f);
    }
}
