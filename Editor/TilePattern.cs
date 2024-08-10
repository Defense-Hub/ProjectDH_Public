using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TilePattern : EditorWindow
{
    private float activeDelay;
    [MenuItem("DevTools/TilePattern &t")]
    private static void ShowWindow()
    {
        TilePattern w = GetWindow<TilePattern>(false, "TilePattern", true);
        w.minSize = new Vector2(400, 300);
        w.maxSize = new Vector2(1600, 1200);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Lava", EditorStyles.boldLabel);
        activeDelay = EditorGUILayout.FloatField("ActiveDelay", activeDelay);

        if (GUILayout.Button("ActiveLava"))
        {
            GameManager.Instance.UnitSpawn.Controller.ActivateLava(activeDelay);
        }

        if (GUILayout.Button("DeActiveLava"))
        {
            GameManager.Instance.UnitSpawn.Controller.DeActivateLava();
        }
    }
}
