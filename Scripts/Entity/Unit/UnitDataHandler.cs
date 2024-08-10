using System;
using UnityEngine;

[Serializable]
public class UnitData
{
    public string Name;
    public string Description;
    public EUnitRank UnitRank;
}

public class UnitDataHandler
{
    // 사용되는 불변의 Data = Data
    [SerializeField] public UnitData Data { get; private set; } = new UnitData();

    public void InitData(UnitBaseSO baseStat)
    {
        Data.Name = baseStat.Name;
        Data.Description = baseStat.UnitDescription;
        Data.UnitRank = baseStat.UnitRank;
    }

    public void InitData(UnitBase baseStat)
    {
        Data.Name = baseStat.Name;
        Data.Description = baseStat.Description;
        Data.UnitRank = baseStat.UnitRank;
    }
}
