using System;
using UnityEngine;

[Serializable]
public class UnitStat
{
    public float MoveSpeed;
    public float AttackPower;
    public float AttackSpeed;
    public float AttackRange;
    public EAttackType AttackType;
    public SpecialAttack SpecialAttack;
}

// TODO : SpecialAttack 지속 시간, 확률

public class UnitStatHandler
{
    // 실제 사용되는 변동 Data = Stat
    [SerializeField] public UnitStat BaseStat { get; private set; }
    [SerializeField] public UnitStat CurrentStat { get; private set; }

    //Todo : 스탯 최소치 설정
    public void Init(UnitBaseSO baseStat)
    {
        BaseStat = new UnitStat();
        CurrentStat = new UnitStat();

        // BaseStat을 
        BaseStat.MoveSpeed = baseStat.BaseMoveSpeed;
        BaseStat.AttackPower = baseStat.BaseAttackPower;
        BaseStat.AttackSpeed = baseStat.BaseAttackSpeed;
        BaseStat.AttackRange = baseStat.BaseAttackRange;
        BaseStat.AttackType = baseStat.AttackType;
        BaseStat.SpecialAttack = baseStat.SpecialAttack;

        UpdateCharacterStat();
    }

    public void Init(UnitBase baseStat)
    {
        BaseStat = new UnitStat();
        CurrentStat = new UnitStat();

        // BaseStat을 
        BaseStat.MoveSpeed = baseStat.BaseMoveSpeed;
        BaseStat.AttackPower = baseStat.BaseAttackPower;
        BaseStat.AttackSpeed = baseStat.BaseAttackSpeed;
        BaseStat.AttackRange = baseStat.BaseAttackRange;
        BaseStat.AttackType = baseStat.AttackType;
        BaseStat.SpecialAttack = baseStat.SpecialAttack;

        UpdateCharacterStat();
    }

    private void InitStat(UnitStat baseStat)
    {
        CurrentStat.MoveSpeed = baseStat.MoveSpeed;
        CurrentStat.AttackPower = baseStat.AttackPower;
        CurrentStat.AttackSpeed = baseStat.AttackSpeed;
        CurrentStat.AttackRange = baseStat.AttackRange;
        CurrentStat.AttackType = baseStat.AttackType;
        CurrentStat.SpecialAttack = baseStat.SpecialAttack;
    }

    public void EnhanceBaseStat(float delta)
    {
        // BaseStat을 
        BaseStat.AttackPower *= delta != 0 ? delta : 1;

        BaseStat.AttackPower= Mathf.Floor(BaseStat.AttackPower);
        //BaseStat.MoveSpeed *= modifier.MoveSpeed != 0 ? modifier.MoveSpeed : 1;
        //BaseStat.AttackSpeed *= modifier.AttackSpeed != 0 ? modifier.AttackSpeed : 1;

        UpdateCharacterStat();
    }

    // 버프 중첩이 발생할 수 있지 않을까??
    // CurrentStat을 기본 Base로 초기화 후 적용
    public void UpdateCharacterStat()
    {
        // 초기화
        InitStat(BaseStat);

        // Field에 적용되어 있는 버프 Apply
        foreach (BuffInfo modifier in GameManager.Instance.FieldManager.BuffDict.Values) 
        {
            ApplyBuffModifier(modifier.BuffSO);
        }
    }

    private void ApplyBuffModifier(BuffSO modifier)
    {
        switch (modifier.StatChangeType)
        {
            case EStatChangeType.Add:
                SetAddModifier(modifier);
                break;
            case EStatChangeType.Multiple:
                SetMultiModifier(modifier);
                break;
            case EStatChangeType.Override:
                SetOverrideModifier(modifier);
                break;
        }
        
    }

    // 특정 값만 변경하고자 할 때, 
    private void SetAddModifier(BuffSO modifier)
    {
        CurrentStat.MoveSpeed += modifier.MoveSpeedDelta != 0 ? modifier.MoveSpeedDelta : 0;
        CurrentStat.AttackPower += modifier.AttackPowerDelta != 0 ? modifier.AttackPowerDelta : 0;
        CurrentStat.AttackSpeed += modifier.AttackSpeedDelta != 0 ? modifier.AttackSpeedDelta : 0;
        CurrentStat.AttackRange += modifier.AttackRangeDelta != 0 ? modifier.AttackRangeDelta : 0;
    }

    private void SetMultiModifier(BuffSO modifier)
    {
        CurrentStat.MoveSpeed *= modifier.MoveSpeedDelta != 0 ? modifier.MoveSpeedDelta : 1;
        CurrentStat.AttackPower *= modifier.AttackPowerDelta != 0 ? modifier.AttackPowerDelta : 1;
        CurrentStat.AttackSpeed *= modifier.AttackSpeedDelta != 0 ? modifier.AttackSpeedDelta : 1;
        CurrentStat.AttackRange *= modifier.AttackRangeDelta != 0 ? modifier.AttackRangeDelta : 1;
    }

    private void SetOverrideModifier(BuffSO modifier)
    {
        CurrentStat.MoveSpeed = modifier.MoveSpeedDelta != 0 ? modifier.MoveSpeedDelta : CurrentStat.MoveSpeed;
        CurrentStat.AttackPower = modifier.AttackPowerDelta != 0 ? modifier.AttackPowerDelta : CurrentStat.AttackPower;
        CurrentStat.AttackSpeed = modifier.MoveSpeedDelta != 0 ? modifier.MoveSpeedDelta : CurrentStat.AttackSpeed;
        CurrentStat.AttackRange = modifier.AttackRangeDelta != 0 ? modifier.AttackRangeDelta : CurrentStat.AttackRange;
        //CurrentStat.AttackType = modifier.AttackType != EAttackType.Default ? modifier.AttackType : CurrentStat.AttackType;
        //CurrentStat.SpecialAttackType = modifier.SpecialAttackType != ESpecialAttackType.Default ? modifier.SpecialAttackType : CurrentStat.SpecialAttackType;
    }
    
    
}
