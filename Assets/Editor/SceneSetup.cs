#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Membuat scene MainMenu, Level1, Level2, Level3 otomatis dan memasukkannya ke Build Settings.
// Jalan otomatis kalau Assets/Scenes/MainMenu.unity belum ada. Bisa dijalankan manual: menu Mario > Buat Ulang Semua Scene.
// PENTING (kerja tim): cukup SATU orang (ketua) yang menjalankan ini lalu commit folder Scenes.
[InitializeOnLoad]
public static class SceneSetup
{
    const string Dir = "Assets/Scenes";

    static SceneSetup()
    {
        EditorApplication.delayCall += () =>
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !File.Exists(Dir + "/MainMenu.unity"))
                Build();
        };
    }

    [MenuItem("Mario/Buat Ulang Semua Scene")]
    public static void Build()
    {
        if (!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);

        Make("MainMenu", go => go.AddComponent<MenuUI>());
        for (int i = 0; i < LevelData.All.Length; i++)
        {
            int idx = i;
            Make("Level" + (i + 1), go => { go.AddComponent<LevelBuilder>().levelIndex = idx; });
        }

        var list = new EditorBuildSettingsScene[LevelData.All.Length + 1];
        list[0] = new EditorBuildSettingsScene(Dir + "/MainMenu.unity", true);
        for (int i = 0; i < LevelData.All.Length; i++)
            list[i + 1] = new EditorBuildSettingsScene(Dir + "/Level" + (i + 1) + ".unity", true);
        EditorBuildSettings.scenes = list;

        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(Dir + "/MainMenu.unity");
        Debug.Log("Scene berhasil dibuat. Tekan Play di scene MainMenu.");
    }

    static void Make(string name, System.Action<GameObject> setup)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var go = new GameObject("Bootstrap");
        setup(go);
        EditorSceneManager.SaveScene(scene, Dir + "/" + name + ".unity");
    }
}
#endif
