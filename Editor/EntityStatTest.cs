using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityStatTest : EditorWindow
{
    private Dictionary<int, UnitBase> unitDatas = new Dictionary<int, UnitBase>();
    private Vector2 windowScrollPosition = Vector2.zero;
    private int curIdx = -1;

    [MenuItem("DevTools/EntityStatTest &e")]
    private static void ShowWindow()
    {
        EntityStatTest w = GetWindow<EntityStatTest>(false, "EntityStatTest", true);
        w.minSize = new Vector2(400, 300);
        w.maxSize = new Vector2(1600, 1200);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("인게임 상태에서만 활성화됩니다.", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("한마리 씩 Test해야합니다.", EditorStyles.boldLabel);
        if (!Application.isPlaying) return;
        
        ShowData();   
    }

    private void InitData()
    {
        unitDatas = GameDataManager.Instance.UnitBases;
    }

    private void ShowData()
    {
        InitData();
        var style = new GUIStyle(GUI.skin.button);
        style.alignment = TextAnchor.MiddleLeft;
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical(GUILayout.Width(160));
            {
                this.windowScrollPosition = GUILayout.BeginScrollView(windowScrollPosition, "box", GUILayout.Width(160));
                foreach (var unitData in unitDatas)
                {
                    var selection = GUILayout.Toggle(curIdx == unitData.Key, new GUIContent($"{unitData.Value.Name} ({unitData.Key})"), style, GUILayout.Width(160));
                    if (selection) curIdx = unitData.Key;
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical(GUILayout.Width(200));
            {
                ShowUnitDetail();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    private void ShowUnitDetail()
    {
        EditorGUILayout.LabelField("유닛 정보", EditorStyles.boldLabel);
        if (curIdx != -1)
        {
            EditorGUILayout.LabelField("# Unit Data (변동 불가)", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField($"Unit ID : {unitDatas[curIdx].ID}");
            EditorGUILayout.LabelField($"Unit 이름 : {unitDatas[curIdx].Name}");
            EditorGUILayout.LabelField($"Unit Rank : {unitDatas[curIdx].UnitRank}");
            EditorGUILayout.LabelField($"Unit 설명 : {unitDatas[curIdx].Description}");

            GUILayout.Space(10);
            EditorGUILayout.LabelField("# Unit Stat", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            unitDatas[curIdx].BaseMoveSpeed = EditorGUILayout.FloatField("이동속도", unitDatas[curIdx].BaseMoveSpeed);
            unitDatas[curIdx].BaseAttackPower = EditorGUILayout.FloatField("공격력", unitDatas[curIdx].BaseAttackPower);
            unitDatas[curIdx].BaseAttackSpeed = EditorGUILayout.FloatField("공격속도", unitDatas[curIdx].BaseAttackSpeed);
            unitDatas[curIdx].BaseAttackRange = EditorGUILayout.FloatField("공격범위", unitDatas[curIdx].BaseAttackRange);
            unitDatas[curIdx].AttackType = (EAttackType)EditorGUILayout.EnumPopup("공격 타입", unitDatas[curIdx].AttackType); 
            GUILayout.Space(10);
            EditorGUILayout.LabelField("# Unit SpecialAttack", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUI.indentLevel++; // 들여쓰기
            EditorGUILayout.LabelField("# Splash", EditorStyles.boldLabel);
            unitDatas[curIdx].SpecialAttack.Splash.Probabillity = EditorGUILayout.IntField("Splash 확률", unitDatas[curIdx].SpecialAttack.Splash.Probabillity);
            unitDatas[curIdx].SpecialAttack.Splash.SplashRange = EditorGUILayout.FloatField("Splash 범위", unitDatas[curIdx].SpecialAttack.Splash.SplashRange);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("# Stun", EditorStyles.boldLabel);
            unitDatas[curIdx].SpecialAttack.Stun.Probabillity = EditorGUILayout.IntField("Stun 확률", unitDatas[curIdx].SpecialAttack.Stun.Probabillity);
            unitDatas[curIdx].SpecialAttack.Stun.EffectDuration = EditorGUILayout.FloatField("Stun 지속시간", unitDatas[curIdx].SpecialAttack.Stun.EffectDuration);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("# Slow", EditorStyles.boldLabel);
            unitDatas[curIdx].SpecialAttack.Slow.Probabillity = EditorGUILayout.IntField("Slow 확률", unitDatas[curIdx].SpecialAttack.Slow.Probabillity);
            unitDatas[curIdx].SpecialAttack.Slow.Delta = EditorGUILayout.FloatField("Slow 변화율", unitDatas[curIdx].SpecialAttack.Slow.Delta);
            unitDatas[curIdx].SpecialAttack.Slow.EffectDuration = EditorGUILayout.FloatField("Slow 지속시간", unitDatas[curIdx].SpecialAttack.Slow.EffectDuration);

            EditorGUI.indentLevel--; // 들여쓰기

            EditorGUILayout.LabelField("유닛을 0,0에 뒀는지 확인해주세요!", EditorStyles.boldLabel);
            if (GUILayout.Button("Apply"))
            {
                ApplyUnitStat();
            }
        }
    }

    private void ApplyUnitStat()
    {
        GameDataManager.Instance.UnitBases[curIdx] = unitDatas[curIdx];
        
        // 유닛 재생성
        // 최대 두마리 있으니까 비워버리기.
        GameManager.Instance.System.DEV_SellUnit();
        GameManager.Instance.System.DEV_SellUnit();
        GameManager.Instance.UnitSpawn.DEV_SpawnUnit(curIdx);
    }
}

