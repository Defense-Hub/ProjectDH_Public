using System;
using UnityEngine;

[Serializable]
public class UnitBase : EntityData
{
    [Header("# 썸네일")]
    public Sprite Thumbnail;
    
    [Header("# 유닛 등급")]
    public EUnitRank UnitRank;

    [Header("# 공격 형태")]
    public EAttackType AttackType;

    [Header("# 공격력")]
    public float BaseAttackPower;

    [Header("# 공격속도")]
    [Range(1f, 5f)] public float BaseAttackSpeed;

    [Header("# 공격 사거리")]
    [Range(2f, 7f)] public float BaseAttackRange;

    [Header("# 유닛 설명")]
    public string Description;

    [Header("# 특수 공격")]
    public SpecialAttack SpecialAttack;

    public UnitBase()
    {
    }

    public UnitBase(int id, string name, float baseMoveSpeed, EUnitRank unitRank, EAttackType attackType,
        float baseAttackPower, float baseAttackSpeed, float baseAttackRange, string description, SplashData splash,
        StunData stun, SlowData slow)
    {
        this.ID = id;
        this.Name = name;
        this.BaseMoveSpeed = baseMoveSpeed;
        this.UnitRank = unitRank;
        this.AttackType = attackType;
        this.BaseAttackPower = baseAttackPower;
        this.BaseAttackSpeed = baseMoveSpeed;
        this.BaseAttackRange = baseAttackRange;
        this.Description = description;

        SpecialAttack = new SpecialAttack();
        this.SpecialAttack.Splash = splash;
        this.SpecialAttack.Stun = stun;
        this.SpecialAttack.Slow = slow;
    }

    public UnitBase GetUnitData()
    {
        return new UnitBase(ID, Name, BaseMoveSpeed, UnitRank, AttackType, BaseAttackPower, BaseAttackSpeed,
            BaseAttackRange, Description, SpecialAttack.Splash, SpecialAttack.Stun, SpecialAttack.Slow);
    }
}