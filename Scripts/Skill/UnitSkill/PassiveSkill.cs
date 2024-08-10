using System;
using UnityEngine;

public class PassiveSkill : UnitSkill
{
    [Header("# 스킬 시전 확률 (1 ~ 10000)")]
    [SerializeField] private int probability;
    public int Probability => probability;

    public override void EndSkill()
    {
        // unit의 curPassiveSkill 비우기
        unit.SkillHandler.CurPassiveSkill = null;
    }
}
