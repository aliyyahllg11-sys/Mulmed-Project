using UnityEngine;
using UnityEngine.SceneManagement;

// Menu utama: pilih level Mudah / Sedang / Sulit. Digambar dengan OnGUI.
public class MenuUI : MonoBehaviour
{
    GUIStyle title, body;

    void Awake()
    {
        var cam = Camera.main;
        if (cam == null)
        {
            var go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            cam = go.AddComponent<Camera>();
            go.AddComponent<AudioListener>();
        }
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.07f, 0.09f, 0.23f);
    }

    void Update()
    {
        for (int i = 1; i <= 3; i++)
            if (GameInput.NumberDown(i)) GameManager.NewGame(i - 1);
    }

    void OnGUI()
    {
        if (title == null)
        {
            title = new GUIStyle(GUI.skin.label) { fontSize = 40, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            title.normal.textColor = new Color(1f, 0.78f, 0.24f);
            body = new GUIStyle(GUI.skin.label) { fontSize = 16, alignment = TextAnchor.MiddleCenter, wordWrap = true };
            body.normal.textColor = Color.white;
        }
        float s = Screen.height / 540f;
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1f));
        float vw = Screen.width / s;
        float w = 520, x = (vw - w) / 2f;

        GUI.Label(new Rect(x, 40, w, 60), "Petualangan Lompat", title);
        GUI.Label(new Rect(x, 105, w, 50), "Lompati celah, injak musuh, kumpulkan koin, dan capai bendera di ujung level.", body);

        Color old = GUI.backgroundColor;
        for (int i = 0; i < LevelData.All.Length; i++)
        {
            LevelDef d = LevelData.All[i];
            GUI.backgroundColor = d.diffColor;
            if (GUI.Button(new Rect(x, 180 + i * 78, w, 66), (i + 1) + ".  " + d.name + "  -  " + d.diff + "\n" + d.desc))
                GameManager.NewGame(i);
        }
        GUI.backgroundColor = old;

        GUI.Label(new Rect(x, 430, w, 70), "Gerak: panah kiri/kanan atau A/D.  Lompat: Spasi, panah atas, atau W.\nEsc: kembali ke menu.  Tekan 1, 2, atau 3 untuk memilih level.", body);
    }
}
