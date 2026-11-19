using UnityEngine;

// Pemain: lari, lompat (coyote time + jump buffer + lompat variabel), terluka, respawn di checkpoint.
public class PlayerController : MonoBehaviour
{
    public float speed = 6f;            // tile per detik
    public float jumpVelocity = 16.8f;  // tinggi lompat sekitar 3,6 tile
    public float gravityScale = 4f;
    public Transform eye;               // opsional, hanya hiasan

    Rigidbody2D rb;
    BoxCollider2D col;
    SpriteRenderer sr;
    float coyote, jumpBuffer, invincible, bounceGrace;
    bool grounded;
    MovingPlatform platform;

    public float Bottom { get { return col.bounds.min.y; } }
    public bool Grounded { get { return grounded; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        var mat = new PhysicsMaterial2D("NoFriction");
        mat.friction = 0f;
        mat.bounciness = 0f;
        col.sharedMaterial = mat;
    }

    bool Playing()
    {
        var gm = GameManager.Instance;
        return gm == null || gm.State == GameState.Play;
    }

    void Update()
    {
        bool playing = Playing();
        if (playing && GameInput.JumpDown) jumpBuffer = 0.12f;

        if (invincible > 0f)
        {
            invincible -= Time.deltaTime;
            sr.enabled = ((int)(invincible * 12f)) % 2 == 0;
        }
        else sr.enabled = true;

        if (playing && transform.position.y < -4f) Hurt(true); // jatuh ke celah
    }

    void FixedUpdate()
    {
        bool playing = Playing();
        grounded = CheckGround(out platform);
        coyote = grounded ? 0.1f : coyote - Time.fixedDeltaTime;
        jumpBuffer -= Time.fixedDeltaTime;
        if (bounceGrace > 0f) bounceGrace -= Time.fixedDeltaTime;

        Vector2 v = Phys.GetVel(rb);
        float h = playing ? GameInput.Horizontal : 0f;
        v.x = h * speed;
        if (platform != null) v.x += platform.VelocityX;

        if (h != 0f && eye != null)
        {
            var lp = eye.localPosition;
            lp.x = Mathf.Abs(lp.x) * (h > 0f ? 1f : -1f);
            eye.localPosition = lp;
        }

        if (playing && jumpBuffer > 0f && coyote > 0f)
        {
            v.y = jumpVelocity;
            jumpBuffer = 0f;
            coyote = 0f;
        }
        // lepas tombol lompat = lompatan lebih pendek
        if (!GameInput.JumpHeld && v.y > 7.5f && bounceGrace <= 0f) v.y = 7.5f;
        v.y = Mathf.Max(v.y, -20f);
        Phys.SetVel(rb, v);
    }

    bool CheckGround(out MovingPlatform plat)
    {
        plat = null;
        Bounds b = col.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y + 0.02f);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.04f);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, 0f, Vector2.down, 0.08f);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D c = hits[i].collider;
            if (c == col || c.isTrigger) continue;
            if (hits[i].normal.y < 0.5f) continue;
            if (c.GetComponent<Enemy>() != null) continue;
            plat = c.GetComponent<MovingPlatform>();
            return true;
        }
        return false;
    }

    // Dipanggil saat menginjak musuh.
    public void Bounce()
    {
        Vector2 v = Phys.GetVel(rb);
        v.y = 13f;
        Phys.SetVel(rb, v);
        bounceGrace = 0.2f;
        jumpBuffer = 0f;
    }

    public void Hurt(bool force = false)
    {
        var gm = GameManager.Instance;
        if (gm != null && gm.State != GameState.Play) return;
        if (!force && invincible > 0f) return;
        if (gm != null)
        {
            gm.LoseLife();
            if (gm.State != GameState.Play) return;
        }
        Respawn();
    }

    void Respawn()
    {
        var gm = GameManager.Instance;
        Vector2 p = gm != null ? gm.Checkpoint : (Vector2)transform.position;
        rb.position = p;
        transform.position = p;
        Phys.SetVel(rb, Vector2.zero);
        invincible = 1.6f;
    }
}
