using UnityEngine;

// Musuh patroli: berbalik di tembok atau tepi jurang. Diinjak dari atas = mati (+50). Disentuh dari samping = pemain terluka.
public class Enemy : MonoBehaviour
{
    public float speed = 1.3f;

    Rigidbody2D rb;
    BoxCollider2D col;
    int dir = -1;
    bool dead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    bool HitSolid(Vector2 origin, Vector2 direction, float dist)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, dist);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D c = hits[i].collider;
            if (c == null || c.isTrigger || c == col) continue;
            if (c.GetComponent<PlayerController>() != null) continue;
            return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        if (dead) return;
        Bounds b = col.bounds;
        float side = b.extents.x + 0.06f;
        // tembok di depan?
        if (HitSolid(new Vector2(b.center.x, b.center.y), new Vector2(dir, 0f), side)) dir = -dir;
        // jurang di depan?
        else if (!HitSolid(new Vector2(b.center.x + dir * side, b.min.y + 0.05f), Vector2.down, 0.5f)) dir = -dir;

        Vector2 v = Phys.GetVel(rb);
        v.x = dir * speed;
        Phys.SetVel(rb, v);
    }

    void OnCollisionEnter2D(Collision2D c) { Touch(c); }
    void OnCollisionStay2D(Collision2D c) { Touch(c); }

    void Touch(Collision2D c)
    {
        if (dead) return;
        PlayerController p = c.collider.GetComponent<PlayerController>();
        if (p == null) return;
        if (p.Bottom > col.bounds.center.y)
        {
            Die();
            p.Bounce();
        }
        else p.Hurt();
    }

    void Die()
    {
        dead = true;
        if (GameManager.Instance != null) GameManager.Instance.AddScore(50);
        col.enabled = false;
        rb.simulated = false;
        var t = transform;
        t.localScale = new Vector3(t.localScale.x, t.localScale.y * 0.3f, 1f);
        t.position += Vector3.down * 0.2f;
        Destroy(gameObject, 0.4f);
    }
}
