using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Buff", fileName = "BuffSO", order = 0)]
public class BuffSO : ScriptableObject
{
    [Header("# Id")]
    public int Id;

    [Header("# Buff타입")]
    public EStatChangeType StatChangeType;

    [Header("# 이동속도 변경")]
    public float MoveSpeedDelta;

    [Header("# 공격력 변경")]
    public float AttackPowerDelta;

    [Header("# 공격속도 변경")]
    public float AttackSpeedDelta;

    [Header("# 공격범위 변경")]
    public float AttackRangeDelta;
}
