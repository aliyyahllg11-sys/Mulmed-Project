using UnityEngine;

public class Coin : MonoBehaviour
{
    Vector3 start;
    void Start() { start = transform.position; }
    void Update() { transform.position = start + Vector3.up * Mathf.Sin(Time.time * 4f + start.x) * 0.06f; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        if (GameManager.Instance != null) GameManager.Instance.AddCoin();
        Destroy(gameObject);
    }
}
