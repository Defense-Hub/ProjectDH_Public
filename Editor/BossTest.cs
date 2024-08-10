using UnityEditor;

[CustomEditor(typeof(StageManager))]
public class BossTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("제작중!", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("TODO : 게임 중 원하는 보스 소환", EditorStyles.boldLabel);
        StageManager editorStageManager = (StageManager)target;
        editorStageManager.TestMode = EditorGUILayout.Toggle("BossTest", editorStageManager.TestMode);
        EditorGUI.BeginDisabledGroup(!editorStageManager.TestMode);
        editorStageManager.TestWaveTime = EditorGUILayout.IntField("Wave 시간", editorStageManager.TestWaveTime);
        editorStageManager.DEV_BossType = (DEV_BossType)EditorGUILayout.EnumPopup("Boss Type", editorStageManager.DEV_BossType);
        editorStageManager.DEV_BossData = new EnemyData(
            EditorGUILayout.FloatField("Boss 이동 속도", editorStageManager.DEV_BossData.MoveSpeed),
            EditorGUILayout.FloatField("Boss 최대 체력", editorStageManager.DEV_BossData.MaxHealth),
            EditorGUILayout.FloatField("Boss 강인함", editorStageManager.DEV_BossData.Toughness)
            );
        EditorGUI.EndDisabledGroup();

        EditorUtility.SetDirty(target);
        // 버튼 만들어서 버튼을 눌렀을 때, 위 정보들을 바탕으로 보스가 나오게?
        // 여러 보스들을 게임을 끄지 않은 상태에서 체크할 수 있음
    }
}
