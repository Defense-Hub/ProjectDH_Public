using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitSpawnManager : MonoBehaviour
{
    [field:Header("# Spawn Info")]
    [field:SerializeField] public int MaxSpawnLevel { get; set; }
    
    public UnitTileController Controller { get; set; }

    public Dictionary<int, int> SpawnEnemyID { get; private set; }  = new Dictionary<int, int>();
    private void Start()
    {
        GameManager.Instance.UnitSpawn = this;
    }
    
    public Unit SpawnRankRandomUnit(int rank)
    {
        Unit spawnUnit = null;
        int rand = Random.Range(GameDataManager.Instance.UnitMinMaxID[rank].minMaxIDs[0],
            GameDataManager.Instance.UnitMinMaxID[rank].minMaxIDs[1]);
        spawnUnit = GameManager.Instance.Pool.SpawnFromPool(rand).ReturnMyComponent<Unit>();

        
        spawnUnit.Init();

        if (!SpawnEnemyID.TryAdd(spawnUnit.Id, 1))
        {
            SpawnEnemyID[spawnUnit.Id]++;
        }
        
        CheckMaterialUnit(spawnUnit.Id, true);

        GameManager.Instance.System.Enforce.EnforceSpawnUnit(spawnUnit);
        if(spawnUnit.DataHandler.Data.UnitRank >= EUnitRank.Unique)
            SummonUIOpen(spawnUnit.DataHandler.Data);
        return spawnUnit;
    }

    private async void SummonUIOpen(UnitData unitData)
    {
        UI_SummonPopup summonPopup = await UIManager.Instance.ShowPopupUI<UI_SummonPopup>(EUIRCode.UI_Summon);
        summonPopup.InitSummonUnitData(unitData);
        summonPopup.Init();
    }

    public void SpawnUnit() // 소환
    {
        if (!Controller.IsAvailableSpawn())
        {
            Debug.LogWarning("Summon => Summon Tile is Full");
            return;
        }
        
        // int rank = RandomEvent.GetArrRandomResult(SpawnPercent);
        // 데이터 적용 
        int rank = RandomEvent.GetArrRandomResult(GameDataManager.Instance.SpawnProbability[PlayerDataManager.Instance.SpawnLevel ].percent);
        
        Controller.SetUnitTile(SpawnRankRandomUnit(rank));
    }

    public Unit DEV_SpawnRankRandomUnit(int TestKey)
    {
        Unit spawnUnit = null;

        
        spawnUnit = GameManager.Instance.Pool.SpawnFromPool(TestKey).ReturnMyComponent<Unit>();

        spawnUnit.Init();

        CheckMaterialUnit(spawnUnit.Id, true);
        
        GameManager.Instance.System.Enforce.EnforceSpawnUnit(spawnUnit);
        
        return spawnUnit;
    }

    public void DEV_SpawnUnit(int testKey) // 소환
    {
        if (!Controller.IsAvailableSpawn())
        {
            Debug.LogWarning("Summon => Summon Tile is Full");
            return;
        }

        if (!SpawnEnemyID.TryAdd(testKey, 1))
        {
            SpawnEnemyID[testKey]++;
        }
        
        
        CheckMaterialUnit(testKey, true);
        
        Controller.SetUnitTile(DEV_SpawnRankRandomUnit(testKey));
    }

    public void TargetUnitSpawn(int id)
    {
        Unit spawnUnit = GameManager.Instance.Pool.SpawnFromPool(id).ReturnMyComponent<Unit>();
        if (spawnUnit == null)
        {
            Debug.LogError($"{id} is not exist");
            return;
        }
        
        spawnUnit.Init();
        if (spawnUnit.DataHandler.Data.UnitRank >= EUnitRank.Unique)
            SummonUIOpen(spawnUnit.DataHandler.Data);
        
        if (!SpawnEnemyID.TryAdd(spawnUnit.Id, 1))
        {
            SpawnEnemyID[spawnUnit.Id]++;
        }
        
        GameManager.Instance.System.Enforce.EnforceSpawnUnit(spawnUnit);
        
        CheckMaterialUnit(spawnUnit.Id, true);
        Controller.SetUnitTile(spawnUnit);
    }

    public void CheckMaterialUnit(int id, bool isCheck)
    {
        if ((isCheck && SpawnEnemyID[id] > 1) || (!isCheck &&  SpawnEnemyID[id] > 0))
        {
            return;
        }
        
        GameManager.Instance.System.Mission.CheckUnitMission(id, isCheck);
        GameManager.Instance.System.Combination.CheckAdvancedCombinationMaterials(id, isCheck);
    }
    
}
