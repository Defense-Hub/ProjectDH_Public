using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceSystem
{
    private List<UnitEnforceData> advancedEnforceDataList = GameDataManager.Instance.AdvancedUnitEnforceDatas;
    private List<UnitEnforceData> basicEnforceDataList = GameDataManager.Instance.BasicUnitEnforceDatas;
    private PlayerInGameData inGameData = PlayerDataManager.Instance.inGameData;
    
    public int GetAdvanceEnforceRequiredCurrency
    {
        get
        {
            int enforceLevel =inGameData.EnhancementData.advancedEnhancementLevel ;
            if (enforceLevel >= advancedEnforceDataList.Count)
                return 0;
            return advancedEnforceDataList[enforceLevel].RequiredCurrency;
        }
    }
    
    public int GetBasicEnforceRequiredCurrency
    {
        get
        {
            int enforceLevel = inGameData.EnhancementData.basicEnhancementLevel ;
            if (enforceLevel >= basicEnforceDataList.Count)
                return 0;
            return basicEnforceDataList[enforceLevel].RequiredCurrency;
        }
    }
    
    
    public void ApplyEnforceAdvancedUnit()
    {
        int enforceLevel = inGameData.EnhancementData.advancedEnhancementLevel - 1;

        ApplyEnforceUnit(enforceLevel, advancedEnforceDataList,
            unit => unit.DataHandler.Data.UnitRank >= EUnitRank.Legendary);
    }

    public void ApplBasicAdvancedUnit()
    {
        int enforceLevel = inGameData.EnhancementData.basicEnhancementLevel - 1;

        ApplyEnforceUnit(enforceLevel, basicEnforceDataList,
            unit => unit.DataHandler.Data.UnitRank < EUnitRank.Legendary);
    }

    private void ApplyEnforceUnit(int enforceLevel, List<UnitEnforceData> enforceList, Predicate<Unit> condition)
    {
        List<UnitTile> unitTiles = GameManager.Instance.UnitSpawn.Controller.UnitTiles;

        foreach (var tile in unitTiles)
        {
            if (tile.SpawnUnits[0] != null && condition.Invoke(tile.SpawnUnits[0]))
            {
                for (int i = 0; i < tile.UnitCount; i++)
                {
                    tile.SpawnUnits[i].StatHandler
                        .EnhanceBaseStat(enforceList[enforceLevel].EnforceValue);
                }
            }
        }
    }

    public void EnforceSpawnUnit(Unit unit)
    {
        if ((int)unit.DataHandler.Data.UnitRank >= (int)EUnitRank.Legendary)
        {
            int enforceLevel = inGameData.EnhancementData.advancedEnhancementLevel - 1;

            ApplyEnforceSpawnUnit(enforceLevel, advancedEnforceDataList, unit);
        }
        else
        {
            int enforceLevel = inGameData.EnhancementData.basicEnhancementLevel - 1;

            ApplyEnforceSpawnUnit(enforceLevel, basicEnforceDataList, unit);
        }
    }

    private void ApplyEnforceSpawnUnit(int enforceLevel, List<UnitEnforceData> enforceList, Unit unit)
    {
        for (int i = 0; i < enforceLevel; i++)
        {
            unit.StatHandler.EnhanceBaseStat(enforceList[i].EnforceValue);
        }
    }

    public void EnforceSpawnProbability()
    {
        int enforceCurrency = GameDataManager.Instance.GameCurrencyData.SpawnProbabilityEnforceGold;

        if (!inGameData.HasGold(enforceCurrency))
        {
            Debug.LogWarning("재화가 부족합니다 !");
            return;
        }
        
        inGameData.ChangeGold(-enforceCurrency);
        
        inGameData.SpawnProbabilityLevelUp();
    }

    public bool CanEnforceSpawnProbability()
    {
        if (!inGameData.HasGold(GameDataManager.Instance.GameCurrencyData.SpawnProbabilityEnforceGold))
            return false;
        return true;
    }
}