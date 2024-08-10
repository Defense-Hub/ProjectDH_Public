using System;
using UnityEngine;

[Serializable]
public class UnitEnhancementData
{
    // 일반 강화 레벨
    public int basicEnhancementLevel = 1;
    // 초월 + 에픽 강화 레벨
    public int advancedEnhancementLevel = 1;
}

[Serializable]
public class PlayerInGameData
{
    // 플레이어의 골드 수
    [field: SerializeField] public int Gold { get; private set; } = 0;
    // 강화 보석
    [field: SerializeField] public int GemStone { get; private set; } = 0;
    
    // 플레이어가 진행 중인 스테이지 레벨
    [field: SerializeField] public int StageLevel { get; private set; } = 1;
    // 유닛 소환 확률 레벨
    [field: SerializeField] public int SpawnProbabilityLevel { get; private set; } = 0;
    // 유닛 강화 데이터
    [field: SerializeField] public UnitEnhancementData EnhancementData { get; private set; } = new UnitEnhancementData();
    
    [field: SerializeField] public int UnitSpawnRequriedGold { get; private set; } = 0;
    
    // private readonly int IncreaseUnitSpawnGold = 2;

    public event Action<int> OnChangeGold;
    public event Action<int> OnChangeGemStone;

    public void ChangeGold(int amount)
    {
        Gold += amount;        
        OnChangeGold?.Invoke(Gold);
    }

    public bool HasGold(int amount)
    {
        return amount <= Gold;
    }

    public void ChangeGemStone(int amount)
    {
        GemStone += amount;
        OnChangeGemStone?.Invoke(GemStone);
    }
    
    public bool HasGemStone(int amount)
    {
        return amount <= GemStone;
    }
    
    public void StageLevelUp()
    {
        StageLevel++;
    }
    
    public void SpawnProbabilityLevelUp()
    {
        if (GameDataManager.Instance.SpawnProbability.Count <= SpawnProbabilityLevel)
        {
            Debug.LogWarning("소환 레벨이 최대치입니다 !");
            return;
        }
        
        SpawnProbabilityLevel++;
    }
    
    public bool BasicUnitEnhancementLevelUp()
    {
        if (GameDataManager.Instance.BasicUnitEnforceDatas.Count <= EnhancementData.basicEnhancementLevel)
        {
            Debug.LogWarning("일반 강화 레벨이 최대치입니다 !");
            return false;
        }
        
        EnhancementData.basicEnhancementLevel++;
        return true;
    }
    
    public bool AdvancedUnitEnhancementLevelUp()
    {
        if (GameDataManager.Instance.AdvancedUnitEnforceDatas.Count <= EnhancementData.advancedEnhancementLevel)
        {
            Debug.LogWarning("고급 강화 레벨이 최대치입니다 !");
            return false;
        }
        
        EnhancementData.advancedEnhancementLevel++;
        return true;
    }

    public void TakeSpawnUnitGold()
    {
        ChangeGold(-UnitSpawnRequriedGold);
        // UnitSpawnRequriedGold += IncreaseUnitSpawnGold;
    }

    public bool IsUnitSpawn()
    {
        return HasGold(UnitSpawnRequriedGold);
    }
    public PlayerInGameData()
    {
        if (GameDataManager.Instance != null)
        {
            if (GameManager.Instance.Tutorial == null)
                Gold = GameDataManager.Instance.GameCurrencyData.InitPlayerGold;
            else
                Gold = 20;
            
            GemStone = GameDataManager.Instance.GameCurrencyData.InitPlayerGemStone;
            StageLevel = 1;
            SpawnProbabilityLevel = 1;
            UnitSpawnRequriedGold = GameDataManager.Instance.GameCurrencyData.InitSummonGold;   
        }
    }
}