using UnityEngine;

// DATA 3 LEVEL. Diporting dari prototipe web. Ubah angka di sini untuk menyetel level.
// Satuan = tile (1 tile = 1 unit Unity). Kolom 0 di kiri. Baris 0 di atas, baris 9-10 = tanah.
//   gaps    : {kolomAwal, kolomAkhir}  celah di tanah (jatuh = kehilangan nyawa)
//   blocks  : {kolom, baris, panjang}  blok bata melayang
//   coins   : {kolom, baris, jumlah}
//   enemies : kolom tempat musuh muncul
//   spikes  : {kolom, lebar}
//   plats   : {kolom, baris, jarakGerak, kecepatan(tile/detik)}  platform bergerak
public class LevelDef
{
    public string name, diff, desc;
    public int cols, check, stairsCol, stairsSteps;
    public float time, enemySpeed;
    public int[][] gaps, blocks, spikes;
    public float[][] coins, plats;
    public int[] enemies;
    public Color diffColor;
}

public static class LevelData
{
    public static readonly LevelDef[] All =
    {
        new LevelDef {
            name = "Level 1", diff = "Mudah", desc = "Celah pendek, musuh pelan, tanpa duri.",
            diffColor = new Color(0.24f, 0.86f, 0.59f),
            cols = 80, time = 150, enemySpeed = 1.3f, check = 40,
            stairsCol = 69, stairsSteps = 3,
            gaps = new[] { new[]{22,23}, new[]{46,47} },
            blocks = new[] { new[]{14,6,3}, new[]{30,6,4}, new[]{38,5,3}, new[]{55,6,3}, new[]{62,6,4} },
            coins = new[] {
                new[]{6f,8f,3f}, new[]{15f,5f,3f}, new[]{22f,7f,2f}, new[]{31f,5f,4f},
                new[]{39f,4f,3f}, new[]{46f,7f,2f}, new[]{56f,5f,3f}, new[]{63f,5f,4f} },
            enemies = new[] { 18, 26, 36, 50, 60 },
            spikes = new int[0][],
            plats = new float[0][]
        },
        new LevelDef {
            name = "Level 2", diff = "Sedang", desc = "Celah lebih lebar, musuh lebih cepat, ada duri.",
            diffColor = new Color(1f, 0.78f, 0.24f),
            cols = 100, time = 140, enemySpeed = 1.7f, check = 50,
            stairsCol = 91, stairsSteps = 3,
            gaps = new[] { new[]{18,20}, new[]{34,36}, new[]{52,54}, new[]{70,72} },
            blocks = new[] { new[]{12,6,3}, new[]{24,6,2}, new[]{28,5,3}, new[]{40,6,3}, new[]{44,4,3},
                             new[]{58,6,4}, new[]{64,5,3}, new[]{80,6,3}, new[]{85,4,2} },
            coins = new[] {
                new[]{5f,8f,3f}, new[]{13f,5f,3f}, new[]{18f,6f,3f}, new[]{29f,4f,3f}, new[]{34f,6f,3f},
                new[]{41f,5f,3f}, new[]{45f,3f,3f}, new[]{52f,6f,3f}, new[]{59f,5f,4f}, new[]{65f,4f,3f},
                new[]{70f,6f,3f}, new[]{81f,5f,3f}, new[]{85f,3f,2f} },
            enemies = new[] { 15, 22, 38, 48, 56, 66, 76, 84 },
            spikes = new[] { new[]{27,2}, new[]{77,2} },
            plats = new float[0][]
        },
        new LevelDef {
            name = "Level 3", diff = "Sulit", desc = "Platform bergerak, banyak musuh dan duri, waktu ketat.",
            diffColor = new Color(1f, 0.42f, 0.42f),
            cols = 130, time = 130, enemySpeed = 2.2f, check = 68,
            stairsCol = 121, stairsSteps = 4,
            gaps = new[] { new[]{14,16}, new[]{24,26}, new[]{36,40}, new[]{52,54}, new[]{64,66},
                           new[]{78,83}, new[]{96,98}, new[]{108,110} },
            blocks = new[] { new[]{10,6,3}, new[]{19,6,3}, new[]{22,4,3}, new[]{44,6,3}, new[]{48,4,3},
                             new[]{58,6,3}, new[]{61,4,2}, new[]{70,6,3}, new[]{74,4,3}, new[]{88,6,3},
                             new[]{92,4,3}, new[]{100,6,3}, new[]{104,4,3}, new[]{114,6,3}, new[]{118,4,3} },
            coins = new[] {
                new[]{5f,8f,3f}, new[]{11f,5f,3f}, new[]{14f,7f,3f}, new[]{20f,5f,3f}, new[]{23f,3f,3f},
                new[]{24f,7f,3f}, new[]{36f,7f,5f}, new[]{45f,5f,3f}, new[]{49f,3f,3f}, new[]{52f,7f,3f},
                new[]{59f,5f,3f}, new[]{62f,3f,2f}, new[]{64f,7f,3f}, new[]{71f,5f,3f}, new[]{75f,3f,3f},
                new[]{78f,7f,6f}, new[]{89f,5f,3f}, new[]{93f,3f,3f}, new[]{96f,7f,3f}, new[]{101f,5f,3f},
                new[]{105f,3f,3f}, new[]{108f,7f,3f}, new[]{115f,5f,3f}, new[]{119f,3f,3f} },
            enemies = new[] { 12, 20, 29, 43, 47, 57, 68, 73, 86, 91, 103, 113, 118 },
            spikes = new[] { new[]{28,2}, new[]{59,2}, new[]{93,2}, new[]{115,2} },
            plats = new[] { new[]{35.5f,9f,4f,2.4f}, new[]{77.5f,9f,5f,2.8f} }
        }
    };
}
