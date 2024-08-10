using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Unit", fileName = "UnitBaseSO", order = 0)]
public class UnitBaseSO : EntityBaseSO
{
    [Header("# 공격력")]
    public float BaseAttackPower;

    [Header("# 공격속도")]
    [Range(1f,5f)]public float BaseAttackSpeed;

    [Header("# 공격 사거리")]
    [Range(2f, 7f)] public float BaseAttackRange;
     
    [Header("# 유닛 설명")]
    public string UnitDescription;

    [Header("# 공격 형태")]
    public EAttackType AttackType;

    [Header("# 특수 공격")]
    public SpecialAttack SpecialAttack;

    [Header("# 유닛 등급")]
    public EUnitRank UnitRank;
}

[Serializable]
public class SpecialAttack
{
    public SplashData Splash;
    public StunData Stun;
    public SlowData Slow;
}

[Serializable]
public class SpecialAttackData
{
    [Range(0, 10000)] public int Probabillity;
}

[Serializable]
public class SplashData : SpecialAttackData
{
    public float SplashRange;
}

[Serializable]
public class StunData : SpecialAttackData
{
    public float EffectDuration;
}

[Serializable]
public class SlowData : SpecialAttackData
{
    public float Delta;
    public float EffectDuration;
}