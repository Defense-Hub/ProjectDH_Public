using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class UnitSpawnProbabilityData
{
    public int[] percent = new int[4];
}

[Serializable]
public class GroupGambleProbability
{
     public List<int> Percentage = new List<int>();
}

[Serializable]
public class GroupGambleProbabilityData
{
    public int GambleProbability;
    public int RequiredGemStone;
}

[Serializable]
public class CombinationData
{
    public int targetID;
    public List<int> materialIDs = new List<int>();
}

[Serializable]
public class MaterialInfo
{
    public int targetID;
    public bool hasUnit;

    public MaterialInfo(int targetID)
    {
        this.targetID = targetID;
        hasUnit = false;
    }
}

[Serializable]
public class MissionData
{
    
    public string missionTitle;
    public string missionDesc;
    public int missionIndex;
    public List<int> materialIDs;
    public int goldReward;
    public int gemStoneReward;

    public MissionData(string missionTitle, string missionDesc, int missionIndex, List<int> materialIDs, int goldReward,
 int gemStoneReward)
    {
        this.missionTitle = missionTitle;
        this.missionDesc = missionDesc;
        this.missionIndex = missionIndex;
        this.goldReward = goldReward;
        this.gemStoneReward = gemStoneReward;
        this.materialIDs = materialIDs;

    }
}

[Serializable]
public class UnitMinMaxIDData
{
    public int[] minMaxIDs;

    public UnitMinMaxIDData(int[] minMaxIDs)
    {
        this.minMaxIDs = new int[2];
        this.minMaxIDs[0] = minMaxIDs[0];
        this.minMaxIDs[1] = minMaxIDs[1];
    }
}

[Serializable]
public class HuntEnemyData
{
    public float MoveSpeed;
    public float MaxHealth;
    public float Toughness;
    public int GemStoneReward;
}

[Serializable]
public class StageData
{
    public int MaxStageLevel;
    public int BossStageLevel;
    public int MaxEnemySpawnNum;
    public float BasicWaveTime;
    public float BossWaveTime;
    public float RecoveryTime;
}

[Serializable]
public class GameCurrencyData
{
    public int InitSummonGold;
    public int SpawnProbabilityEnforceGold;
    public int StageClearBonusGold;
    public int EnemyPerKillGold ;
    public int InitPlayerGold ;
    public int InitPlayerGemStone ;
}

[Serializable]
public class UnitEnforceData
{
    public float EnforceValue;
    public int RequiredCurrency;
}