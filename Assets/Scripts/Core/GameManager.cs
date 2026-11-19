using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Play, Won, Lost }

// Mengatur skor, nyawa, waktu, checkpoint, menang/kalah, dan HUD (digambar dengan OnGUI, jadi tidak butuh Canvas).
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Data yang bertahan antar-scene
    public static int Score, Coins, Lives = 3;

    public GameState State = GameState.Play;
    public int LevelIndex;
    public LevelDef Def;
    public float TimeLeft;
    public Vector2 Checkpoint;
    public PlayerController Player;

    int bonus;
    GUIStyle label, big, small;

    void Awake() { Instance = this; }

    public static void NewGame(int level)
    {
        Score = 0; Coins = 0; Lives = 3;
        SceneManager.LoadScene("Level" + (level + 1));
    }

    public void AddScore(int n) { Score += n; }
    public void AddCoin() { Coins++; Score += 10; }

    public void LoseLife()
    {
        Lives--;
        if (Lives <= 0) { Lives = 0; State = GameState.Lost; }
    }

    public void CompleteLevel()
    {
        if (State != GameState.Play) return;
        bonus = Mathf.CeilToInt(TimeLeft) * 2;
        Score += bonus;
        State = GameState.Won;
    }

    void Update()
    {
        if (GameInput.EscapeDown) { SceneManager.LoadScene("MainMenu"); return; }
        if (State != GameState.Play) return;
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            TimeLeft = 60f;
            if (Player != null) Player.Hurt(true);
        }
    }

    void MakeStyles()
    {
        if (label != null) return;
        label = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold };
        label.normal.textColor = Color.white;
        big = new GUIStyle(GUI.skin.label) { fontSize = 34, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        big.normal.textColor = new Color(1f, 0.78f, 0.24f);
        small = new GUIStyle(GUI.skin.label) { fontSize = 18, alignment = TextAnchor.MiddleCenter };
        small.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        MakeStyles();
        float s = Screen.height / 540f;
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1f));
        float vw = Screen.width / s;

        GUI.Box(new Rect(0, 0, vw, 36), GUIContent.none);
        GUI.Label(new Rect(12, 4, 260, 30), (Def != null ? Def.name + " - " + Def.diff : ""), label);
        GUI.Label(new Rect(vw * 0.36f, 4, 200, 30), "Skor " + Score, label);
        GUI.Label(new Rect(vw * 0.54f, 4, 160, 30), "Koin " + Coins, label);
        GUI.Label(new Rect(vw * 0.70f, 4, 160, 30), "Nyawa " + Lives, label);
        GUI.Label(new Rect(vw - 130, 4, 120, 30), "Waktu " + Mathf.CeilToInt(Mathf.Max(0f, TimeLeft)), label);

        if (State == GameState.Play) return;

        float w = 460, h = 280, x = (vw - w) / 2f, y = (540 - h) / 2f;
        GUI.Box(new Rect(x, y, w, h), GUIContent.none);
        bool last = LevelIndex >= LevelData.All.Length - 1;

        if (State == GameState.Lost)
        {
            GUI.Label(new Rect(x, y + 14, w, 50), "Nyawa habis", big);
            GUI.Label(new Rect(x, y + 70, w, 30), "Skor " + Score + "   Koin " + Coins, small);
        }
        else
        {
            GUI.Label(new Rect(x, y + 14, w, 50), last ? "Game tamat!" : "Level selesai!", big);
            GUI.Label(new Rect(x, y + 70, w, 30), "Bonus waktu +" + bonus + "   Skor total " + Score, small);
        }

        float by = y + 130;
        if (State == GameState.Won && !last)
        {
            if (GUI.Button(new Rect(x + 40, by, w - 80, 40), "Level berikutnya"))
                SceneManager.LoadScene("Level" + (LevelIndex + 2));
            by += 48;
        }
        if (GUI.Button(new Rect(x + 40, by, w - 80, 40), "Ulangi level")) NewGame(LevelIndex);
        by += 48;
        if (GUI.Button(new Rect(x + 40, by, w - 80, 40), "Menu utama")) SceneManager.LoadScene("MainMenu");
    }
}
