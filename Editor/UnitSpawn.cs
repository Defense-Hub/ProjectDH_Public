using UnityEngine;
using UnityEditor;
public class UnitSpawn : EditorWindow
{
    int unitKey;
    [MenuItem("DevTools/UnitSpawn &u")]
    private static void ShowWindow()
    {
        UnitSpawn w = GetWindow<UnitSpawn>(false, "UnitSpawn", true);
        w.minSize = new Vector2(400, 300);
        w.maxSize = new Vector2(1600, 1200);
    }

    private void OnGUI()
    {
        CommonUnitSpawn();
        EditorGUILayout.Space();
        RareUnitSpawn();
        EditorGUILayout.Space();
        UniqueUnitSpawn();
        EditorGUILayout.Space();
        LegendaryUnitSpawn();
        EditorGUILayout.Space();
        EpicUnitSpawn();
        EditorGUILayout.Space();
        SellUnit();
        EditorGUILayout.Space();
        HuntEnemy();
    }

    private void CommonUnitSpawn()
    {
        EditorGUILayout.LabelField("Common Unit 소환", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fire"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(0);
        }
        else if (GUILayout.Button("Ice"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(1);
        }
        else if (GUILayout.Button("Ground"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(2);
        }
        else if (GUILayout.Button("Dark"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(3);
        }
        else if (GUILayout.Button("Water"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(4);
        }
        GUILayout.EndHorizontal();
    }

    private void RareUnitSpawn()
    {
        EditorGUILayout.LabelField("Rare Unit 소환", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fire"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(100);
        }
        else if (GUILayout.Button("Ice"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(101);
        }
        else if (GUILayout.Button("Ground"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(102);
        }
        else if (GUILayout.Button("Dark"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(103);
        }
        else if (GUILayout.Button("Water"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(104);
        }
        GUILayout.EndHorizontal();
    }

    private void UniqueUnitSpawn()
    {
        EditorGUILayout.LabelField("Unique Unit 소환", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fire"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(200);
        }
        else if (GUILayout.Button("Ice"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(201);
        }
        else if (GUILayout.Button("Ground"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(202);
        }
        else if (GUILayout.Button("Dark"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(203);
        }
        else if (GUILayout.Button("Water"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(204);
        }
        GUILayout.EndHorizontal();
    }

    private void LegendaryUnitSpawn()
    {
        EditorGUILayout.LabelField("Legendary Unit 소환", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fire"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(300);
        }
        else if (GUILayout.Button("Ice"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(301);
        }
        else if (GUILayout.Button("Ground"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(302);
        }
        else if (GUILayout.Button("Dark"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(303);
        }
        else if (GUILayout.Button("Water"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(304);
        }
        GUILayout.EndHorizontal();
    }

    private void EpicUnitSpawn()
    {
        EditorGUILayout.LabelField("Epic Unit 소환", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fire"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(400);
        }
        else if (GUILayout.Button("Ice")) 
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(401);
        }
        else if (GUILayout.Button("Ground"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(402);
        }
        else if (GUILayout.Button("Dark"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(403);
        }
        else if (GUILayout.Button("Water"))
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(404);
        }
        GUILayout.EndHorizontal();
    }

    private void SellUnit()
    {
        EditorGUILayout.LabelField("0,0 좌표에 유닛을 두고 사용해야 합니다.", EditorStyles.boldLabel);
        if (GUILayout.Button("판매"))
        {
            GameManager.Instance.System.DEV_SellUnit();
        }
    }

    private void HuntEnemy()
    {
        EditorGUILayout.LabelField("사냥 시작", EditorStyles.boldLabel);
        if (GUILayout.Button("사냥"))
        {
            GameManager.Instance.System.Hunt();
        }
    }
}
