using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneQuickSelect : EditorWindow
{
    [MenuItem("DevTools/SceneQuickSelect/StartScene &1")]
    private static void StartScene()
    {
        // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
        Debug.Log("Switch to StartScene");
    }

    [MenuItem("DevTools/SceneQuickSelect/MainScene &2")]
    private static void MainScene()
    {
        // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
        Debug.Log("Switch to MainScene");
    }

    [MenuItem("DevTools/SceneQuickSelect/BossTest &3")]
    private static void BossScene()
    {
        // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Dev/BossTest.unity");
        Debug.Log("Switch to BossTestScene");
    }

    [MenuItem("DevTools/SceneQuickSelect/UnitTest &4")]
    private static void UnitScene()
    {
        // 현재 씬이 수정 되었으면 저장할 것인지 확인 후 전환
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Dev/HH/UnitTest.unity");
        Debug.Log("Switch to UnitTestScene");
    }
}