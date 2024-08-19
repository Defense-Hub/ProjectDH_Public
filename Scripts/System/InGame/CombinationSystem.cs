using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class CombinationSystem
{
    private GameSystemManager system;
    public Dictionary<int,List<MaterialInfo>> AdvancedCombinationDict { get; private set; } //unit리스트
    
    public CombinationSystem(GameSystemManager system)
    {
        this.system = system;
        
        AdvancedCombinationDict = new Dictionary<int, List<MaterialInfo>>();
        
        foreach (var combination in GameDataManager.Instance.CombinationDatas)
        {
            this.system.InitMaterialDictionary(AdvancedCombinationDict, combination.targetID, combination.materialIDs);
        }
    }
    
    public void CombinationBasicUnit(UnitTile tile)
    {
        if (tile.UnitCount != 2 || tile.SpawnUnits[0].DataHandler.Data.UnitRank > EUnitRank.Unique)
            return;

        EUnitRank rank= tile.SpawnUnits[0].DataHandler.Data.UnitRank;
        
        tile.CombinationUnit();
        
        Unit unit = GameManager.Instance.UnitSpawn.SpawnRankRandomUnit((int)rank + 1);

        tile.SetUnit(unit);
        GameManager.Instance.UnitSpawn.Controller.TileEvents.ReconstructSpawnUnit(tile);
        tile.CallUnBlockSummonBtn();
    }

    public void CombinationAdvancedUnit(int targetID)
    {
        CombinationData data = GameDataManager.Instance.CombinationDatas.Find(data => data.targetID == targetID);
        UnitTile[] mixTiles = new UnitTile[data.materialIDs.Count];

        for (int i = 0; i < mixTiles.Length; i++)
        {
            mixTiles[i] = GameManager.Instance.UnitSpawn.Controller.GetCombinationUnitTile(data.materialIDs[i]);
            if (mixTiles[i] == null)
            {
                Debug.LogWarning("상위 유닛 합성 실패 !");
                return;
            }
        }

        for (int i = 0; i < mixTiles.Length; i++)
        {
            mixTiles[i].DeActivateUnit();
        }
        
        // TODO : targetID 유닛 소환 로직 추가
        GameManager.Instance.UnitSpawn.TargetUnitSpawn(targetID);
    }

    public void CheckAdvancedCombinationMaterials(int id, bool isCheck)
    {
        foreach (var combination in AdvancedCombinationDict)
        {
            system.CheckMaterialDictionary(AdvancedCombinationDict[combination.Key], id, isCheck);
        }
    }
    
}
