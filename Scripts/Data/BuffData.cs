using System;
using UnityEngine;

[Serializable]
public class BuffData
{
    public int ID;
    
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

    public BuffData(int id,EStatChangeType statChangeType, float moveSpeedDelta, float attackPowerDelta, float attackSpeedDelta, float attackRangeDelta)
    {
        this.ID = id;
        this.StatChangeType = statChangeType;
        this.MoveSpeedDelta = moveSpeedDelta;
        this.AttackPowerDelta = attackPowerDelta;
        this.AttackSpeedDelta = attackSpeedDelta;
        this.AttackRangeDelta = attackRangeDelta;
    }
    
    public BuffData GetUnitData()
    {
        return new BuffData(ID,StatChangeType, MoveSpeedDelta,AttackPowerDelta,AttackSpeedDelta,AttackRangeDelta );
    }
}