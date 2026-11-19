using UnityEngine;

// Platform bergerak kiri-kanan. Pemain yang berdiri di atasnya ikut terbawa (lihat PlayerController).
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public float range = 4f;
    public float speed = 2.4f;

    Rigidbody2D rb;
    float x0;
    int dir = 1;

    public float VelocityX { get { return dir * speed; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        x0 = transform.position.x;
    }

    void FixedUpdate()
    {
        float x = rb.position.x;
        if (x > x0 + range) dir = -1;
        else if (x < x0) dir = 1;
        Phys.SetVel(rb, new Vector2(dir * speed, 0f));
    }
}
