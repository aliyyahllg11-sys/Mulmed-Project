using UnityEngine;

// Membuat sprite sederhana lewat kode (kotak, lingkaran, segitiga) supaya project
// bisa jalan tanpa aset gambar. Nanti anggota tim seni bisa menggantinya dengan sprite asli.
public static class SpriteFactory
{
    static Sprite square, circle, triangle;

    public static Sprite Square
    {
        get
        {
            if (square == null)
            {
                var t = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                t.SetPixel(0, 0, Color.white); t.Apply();
                t.filterMode = FilterMode.Point;
                square = Sprite.Create(t, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            }
            return square;
        }
    }

    public static Sprite Circle
    {
        get
        {
            if (circle == null)
            {
                const int n = 32;
                var t = new Texture2D(n, n, TextureFormat.RGBA32, false);
                for (int y = 0; y < n; y++)
                    for (int x = 0; x < n; x++)
                    {
                        float dx = x + 0.5f - n / 2f, dy = y + 0.5f - n / 2f;
                        t.SetPixel(x, y, dx * dx + dy * dy <= (n / 2f) * (n / 2f) ? Color.white : Color.clear);
                    }
                t.Apply();
                circle = Sprite.Create(t, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), n);
            }
            return circle;
        }
    }

    // Segitiga menghadap atas, pivot di tengah bawah, ukuran 1x1 unit.
    public static Sprite Triangle
    {
        get
        {
            if (triangle == null)
            {
                const int n = 32;
                var t = new Texture2D(n, n, TextureFormat.RGBA32, false);
                for (int y = 0; y < n; y++)
                {
                    float half = (1f - (y + 0.5f) / n) * (n / 2f);
                    for (int x = 0; x < n; x++)
                        t.SetPixel(x, y, Mathf.Abs(x + 0.5f - n / 2f) <= half ? Color.white : Color.clear);
                }
                t.Apply();
                triangle = Sprite.Create(t, new Rect(0, 0, n, n), new Vector2(0.5f, 0f), n);
            }
            return triangle;
        }
    }
}
