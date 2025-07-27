using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;  // para SceneManager

public class ReplaceBadMaterials : EditorWindow
{
    Material fallback;

    [MenuItem("Tools/Replace Incompatible Materials")]
    static void OpenWindow() => GetWindow<ReplaceBadMaterials>("Replace Bad Mats");

    void OnGUI()
    {
        fallback = (Material)EditorGUILayout.ObjectField("Fallback Material", fallback, typeof(Material), false);
        if (GUILayout.Button("Run Replacement") && fallback != null)
            ReplaceInScene();
    }

    void ReplaceInScene()
    {
        int replaced = 0;

        // Usamos sceneCount y GetSceneAt en lugar de GetAllScenes()
        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            foreach (GameObject root in scene.GetRootGameObjects())
                replaced += ProcessGO(root);
        }

        EditorSceneManager.MarkAllScenesDirty();
        Debug.Log($"✅ Replaced {replaced} materials with fallback.");
    }

    int ProcessGO(GameObject go)
    {
        int count = 0;
        var mr = go.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            var mats = mr.sharedMaterials;
            bool dirty = false;
            for (int j = 0; j < mats.Length; j++)
            {
                if (mats[j] != fallback && !IsURPCompatible(mats[j]?.shader))
                {
                    mats[j] = fallback;
                    dirty = true;
                    count++;
                }
            }
            if (dirty) mr.sharedMaterials = mats;
        }
        foreach (Transform child in go.transform)
            count += ProcessGO(child.gameObject);
        return count;
    }

    bool IsURPCompatible(Shader s)
    {
        return s != null && s.name.ToLower().Contains("universal render pipeline");
    }
}
