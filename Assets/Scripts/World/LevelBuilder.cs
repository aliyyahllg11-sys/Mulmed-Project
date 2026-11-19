using UnityEngine;

// Membangun satu level dari LevelData saat scene dimulai.
// Scene cukup berisi satu GameObject kosong dengan komponen ini (levelIndex 0, 1, atau 2).
public class LevelBuilder : MonoBehaviour
{
    public int levelIndex;

    const int Rows = 11;          // tinggi level (tile)
    const float GroundTop = 2f;   // permukaan tanah di y = 2

    static readonly Color Dirt  = new Color(0.54f, 0.35f, 0.20f);
    static readonly Color Grass = new Color(0.30f, 0.76f, 0.35f);
    static readonly Color Brick = new Color(0.77f, 0.39f, 0.23f);
    static readonly Color Sky   = new Color(0.45f, 0.78f, 1f);

    Transform root;

    void Awake()
    {
        LevelDef d = LevelData.All[levelIndex];
        root = new GameObject("Level").transform;

        BuildBackground(d);
        BuildGround(d);
        BuildBlocks(d);
        BuildStairs(d);
        BuildWalls(d);
        BuildCoins(d);
        BuildSpikes(d);
        BuildPlatforms(d);
        BuildEnemies(d);
        BuildCheckpoint(d);
        BuildGoal(d);
        PlayerController player = BuildPlayer();
        BuildCameraAndManager(d, player);
    }

    // ---------- helper ----------
    GameObject Box(string name, Vector2 center, Vector2 size, Color color, int order, bool collider, Transform parent = null)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent != null ? parent : root, false);
        go.transform.position = new Vector3(center.x, center.y, 0f);
        go.transform.localScale = new Vector3(size.x, size.y, 1f);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = SpriteFactory.Square;
        sr.color = color;
        sr.sortingOrder = order;
        if (collider) go.AddComponent<BoxCollider2D>();
        return go;
    }

    // ---------- bagian level ----------
    void BuildBackground(LevelDef d)
    {
        for (int x = 4; x < d.cols; x += 13)
        {
            float y = 8f + (x % 3);
            for (int i = 0; i < 3; i++)
            {
                var go = new GameObject("Cloud");
                go.transform.SetParent(root, false);
                go.transform.position = new Vector3(x + i * 0.9f, y + (i == 1 ? 0.35f : 0f), 0f);
                float r = i == 1 ? 1.5f : 1.1f;
                go.transform.localScale = new Vector3(r, r, 1f);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteFactory.Circle;
                sr.color = new Color(1f, 1f, 1f, 0.85f);
                sr.sortingOrder = -10;
            }
        }
    }

    void BuildGround(LevelDef d)
    {
        int x = 0;
        while (x < d.cols)
        {
            if (InGap(d, x)) { x++; continue; }
            int a = x;
            while (x < d.cols && !InGap(d, x)) x++;
            int w = x - a;
            Box("Ground", new Vector2(a + w / 2f, GroundTop / 2f), new Vector2(w, GroundTop), Dirt, 0, true);
            Box("Grass", new Vector2(a + w / 2f, GroundTop - 0.14f), new Vector2(w, 0.28f), Grass, 1, false);
        }
    }

    static bool InGap(LevelDef d, int col)
    {
        foreach (int[] g in d.gaps) if (col >= g[0] && col <= g[1]) return true;
        return false;
    }

    void BuildBlocks(LevelDef d)
    {
        foreach (int[] b in d.blocks)
        {
            // b = {kolom, baris, panjang}; baris r menempati y dari 10-r sampai 11-r
            Box("Brick", new Vector2(b[0] + b[2] / 2f, Rows - 1 - b[1] + 0.5f), new Vector2(b[2], 1f), Brick, 0, true);
        }
    }

    void BuildStairs(LevelDef d)
    {
        for (int i = 0; i < d.stairsSteps; i++)
        {
            float h = i + 1;
            Box("Stair", new Vector2(d.stairsCol + i + 0.5f, GroundTop + h / 2f), new Vector2(1f, h), Dirt, 0, true);
            Box("StairTop", new Vector2(d.stairsCol + i + 0.5f, GroundTop + h - 0.14f), new Vector2(1f, 0.28f), Grass, 1, false);
        }
    }

    void BuildWalls(LevelDef d)
    {
        Box("WallLeft", new Vector2(-0.5f, 6f), new Vector2(1f, 40f), Color.clear, 0, true);
        Box("WallRight", new Vector2(d.cols + 0.5f, 6f), new Vector2(1f, 40f), Color.clear, 0, true);
    }

    void BuildCoins(LevelDef d)
    {
        foreach (float[] c in d.coins)
        {
            for (int i = 0; i < (int)c[2]; i++)
            {
                var go = new GameObject("Coin");
                go.transform.SetParent(root, false);
                go.transform.position = new Vector3(c[0] + i + 0.5f, Rows - 1 - c[1] + 0.5f, 0f);
                go.transform.localScale = new Vector3(0.55f, 0.7f, 1f);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteFactory.Circle;
                sr.color = new Color(1f, 0.78f, 0.24f);
                sr.sortingOrder = 5;
                var cc = go.AddComponent<CircleCollider2D>();
                cc.isTrigger = true;
                cc.radius = 0.5f;
                go.AddComponent<Coin>();
            }
        }
    }

    void BuildSpikes(LevelDef d)
    {
        foreach (int[] s in d.spikes)
        {
            var holder = new GameObject("Spikes");
            holder.transform.SetParent(root, false);
            holder.transform.position = new Vector3(s[0] + s[1] / 2f, GroundTop + 0.2f, 0f);
            var bc = holder.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;
            bc.size = new Vector2(s[1] - 0.2f, 0.4f);
            holder.AddComponent<Hazard>();
            for (int i = 0; i < s[1] * 2; i++)
            {
                var t = new GameObject("Spike");
                t.transform.SetParent(holder.transform, false);
                t.transform.position = new Vector3(s[0] + 0.25f + i * 0.5f, GroundTop, 0f);
                t.transform.localScale = new Vector3(0.5f, 0.55f, 1f);
                var sr = t.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteFactory.Triangle;
                sr.color = new Color(0.79f, 0.81f, 0.91f);
                sr.sortingOrder = 2;
            }
        }
    }

    void BuildPlatforms(LevelDef d)
    {
        foreach (float[] p in d.plats)
        {
            // p = {kolom, baris, jarak, kecepatan}; lebar 2 tile, tebal 0.375
            float top = Rows - p[1];
            var go = Box("MovingPlatform", new Vector2(p[0] + 1f, top - 0.1875f), new Vector2(2f, 0.375f), new Color(0.56f, 0.59f, 0.77f), 2, true);
            var rb = go.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            var mp = go.AddComponent<MovingPlatform>();
            mp.range = p[2];
            mp.speed = p[3];
        }
    }

    void BuildEnemies(LevelDef d)
    {
        var noFriction = new PhysicsMaterial2D("EnemyMat");
        noFriction.friction = 0f;
        foreach (int col in d.enemies)
        {
            var go = Box("Enemy", new Vector2(col + 0.5f, GroundTop + 0.4f), new Vector2(0.81f, 0.75f), new Color(0.54f, 0.30f, 1f), 5, true);
            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 4f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            go.GetComponent<BoxCollider2D>().sharedMaterial = noFriction;
            var e = go.AddComponent<Enemy>();
            e.speed = d.enemySpeed;
            // mata
            for (int i = 0; i < 2; i++)
            {
                var eye = new GameObject("Eye");
                eye.transform.SetParent(go.transform, false);
                eye.transform.localPosition = new Vector3(i == 0 ? -0.22f : 0.22f, 0.1f, 0f);
                eye.transform.localScale = new Vector3(0.22f, 0.26f, 1f);
                var sr = eye.AddComponent<SpriteRenderer>();
                sr.sprite = SpriteFactory.Square;
                sr.color = Color.white;
                sr.sortingOrder = 6;
            }
        }
    }

    void BuildCheckpoint(LevelDef d)
    {
        float x = d.check + 0.5f;
        var pole = Box("Checkpoint", new Vector2(x, GroundTop + 1.6f), new Vector2(0.1f, 3.2f), new Color(0.91f, 0.93f, 1f), 3, false);
        var flag = Box("CheckpointFlag", new Vector2(x + 0.4f, GroundTop + 2.8f), new Vector2(0.8f, 0.5f), new Color(0.49f, 0.53f, 0.72f), 3, false, pole.transform.parent);
        var trigger = new GameObject("CheckpointTrigger");
        trigger.transform.SetParent(root, false);
        trigger.transform.position = new Vector3(x, GroundTop + 3f, 0f);
        var bc = trigger.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.size = new Vector2(1f, 6f);
        var cp = trigger.AddComponent<Checkpoint>();
        cp.flag = flag.GetComponent<SpriteRenderer>();
    }

    void BuildGoal(LevelDef d)
    {
        float x = d.cols - 4 + 0.5f;
        Box("GoalPole", new Vector2(x, GroundTop + 3.5f), new Vector2(0.14f, 7f), new Color(0.91f, 0.93f, 1f), 3, false);
        Box("GoalFlag", new Vector2(x + 0.55f, GroundTop + 6.4f), new Vector2(1.1f, 0.8f), new Color(1f, 0.36f, 0.36f), 3, false);
        var trigger = new GameObject("GoalTrigger");
        trigger.transform.SetParent(root, false);
        trigger.transform.position = new Vector3(x, GroundTop + 4f, 0f);
        var bc = trigger.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.size = new Vector2(0.8f, 8f);
        trigger.AddComponent<GoalFlag>();
    }

    PlayerController BuildPlayer()
    {
        var go = Box("Player", new Vector2(2.5f, GroundTop + 0.5f), new Vector2(0.69f, 0.94f), new Color(1f, 0.54f, 0.24f), 10, true);
        var rb = go.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        var pc = go.AddComponent<PlayerController>();

        // topi
        var hat = new GameObject("Hat");
        hat.transform.SetParent(go.transform, false);
        hat.transform.localPosition = new Vector3(0f, 0.38f, 0f);
        hat.transform.localScale = new Vector3(1.1f, 0.28f, 1f);
        var hsr = hat.AddComponent<SpriteRenderer>();
        hsr.sprite = SpriteFactory.Square;
        hsr.color = new Color(0.1f, 0.71f, 0.65f);
        hsr.sortingOrder = 11;
        // mata
        var eye = new GameObject("Eye");
        eye.transform.SetParent(go.transform, false);
        eye.transform.localPosition = new Vector3(0.2f, 0.12f, 0f);
        eye.transform.localScale = new Vector3(0.3f, 0.22f, 1f);
        var esr = eye.AddComponent<SpriteRenderer>();
        esr.sprite = SpriteFactory.Square;
        esr.color = Color.white;
        esr.sortingOrder = 11;
        pc.eye = eye.transform;
        return pc;
    }

    void BuildCameraAndManager(LevelDef d, PlayerController player)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            cam = camGo.AddComponent<Camera>();
            camGo.AddComponent<AudioListener>();
        }
        cam.orthographic = true;
        cam.orthographicSize = 5.5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Sky;
        var follow = cam.gameObject.GetComponent<CameraFollow>();
        if (follow == null) follow = cam.gameObject.AddComponent<CameraFollow>();
        follow.target = player.transform;
        follow.levelWidth = d.cols;

        var gm = gameObject.AddComponent<GameManager>();
        gm.LevelIndex = levelIndex;
        gm.Def = d;
        gm.TimeLeft = d.time;
        gm.Checkpoint = new Vector2(2.5f, GroundTop + 0.6f);
        gm.Player = player;
    }
}
