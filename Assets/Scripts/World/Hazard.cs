using UnityEngine;

// Duri: sentuh = pemain terluka.
public class Hazard : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        var p = other.GetComponent<PlayerController>();
        if (p != null) p.Hurt();
    }
}
