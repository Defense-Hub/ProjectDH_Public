using System;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelect : EditorWindow
{
    string HHPath = "Assets/Dev/HH/";
    string SDPath = "Assets/Dev/SD/";
    string EJPath = "Assets/Dev/EJ/";
    string SNPath = "Assets/Dev/SN/";

    [MenuItem("DevTools/SceneSelect &d")]
    private static void ShowWindow()
    {
        SceneSelect w = GetWindow<SceneSelect>(false, "SceneSelect", true);
        w.minSize = new Vector2(850, 300);
        w.maxSize = new Vector2(850, 300);
    }

    private void FindScenes(string path)
    {
        string[] sAssetGuids = AssetDatabase.FindAssets("t:SceneAsset", new[] { path });
        string[] sAssetPathList = Array.ConvertAll(sAssetGuids, AssetDatabase.GUIDToAssetPath);

        foreach(string sAssetPath in sAssetPathList)
        {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath(sAssetPath, typeof(SceneAsset)) as SceneAsset;
            if(GUILayout.Button(scene.name, GUILayout.Width(200)))
            {
                // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                }
                EditorSceneManager.OpenScene(sAssetPath);
                Debug.Log($"{scene.name} Open");
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical(GUILayout.Width(200));
        EditorGUILayout.LabelField("현호", EditorStyles.boldLabel, GUILayout.Width(200));
        // Asset/Dev/HH/ 여기 내용 중에 *.Scene
        FindScenes(HHPath);
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical(GUILayout.Width(200));
        EditorGUILayout.LabelField("승도", EditorStyles.boldLabel, GUILayout.Width(200));
        // Dev/SD/에 존재하는 Scene들 전부
        FindScenes(SDPath);
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical(GUILayout.Width(200));
        EditorGUILayout.LabelField("은지", EditorStyles.boldLabel, GUILayout.Width(200));
        // Dev/EJ/에 존재하는 Scene들 전부
        FindScenes(EJPath);
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical(GUILayout.Width(200));
        EditorGUILayout.LabelField("세나", EditorStyles.boldLabel, GUILayout.Width(200));
        // Dev/SN/에 존재하는 Scene들 전부
        FindScenes(SNPath);
        GUILayout.EndVertical();


        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("DEV", EditorStyles.boldLabel);
        if (GUILayout.Button("StartScene"))
        {
            // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
            Debug.Log($"StartScene Open");
        }

        if (GUILayout.Button("MainScene"))
        {
            // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
            Debug.Log($"MainScene Open");
        }

        if (GUILayout.Button("BossScene"))
        {
            // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            EditorSceneManager.OpenScene("Assets/Dev/BossTest.unity");
            Debug.Log($"BossScene Open");
        }
    }
}