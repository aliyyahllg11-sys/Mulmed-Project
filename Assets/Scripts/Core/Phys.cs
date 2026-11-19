using UnityEngine;

// Helper supaya kode jalan di Unity 2021/2022 (velocity) dan Unity 6 (linearVelocity).
public static class Phys
{
    public static Vector2 GetVel(Rigidbody2D rb)
    {
#if UNITY_6000_0_OR_NEWER
        return rb.linearVelocity;
#else
        return rb.velocity;
#endif
    }

    public static void SetVel(Rigidbody2D rb, Vector2 v)
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = v;
#else
        rb.velocity = v;
#endif
    }
}
