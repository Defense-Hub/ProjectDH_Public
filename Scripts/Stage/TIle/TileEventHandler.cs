using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileEventHandler
{
    private UnitTileController controller;
    private HashSet<UnitTile> eventTileHash = new HashSet<UnitTile>();
    public TileEventHandler(UnitTileController controller)
    {
        this.controller = controller;
    }

    public void ReconstructSpawnUnit(UnitTile targetTile) // 1마리 남아있는 유닛 위치 재구성
    {
        Unit remainingUnit = targetTile.SpawnUnits[0];

        if (remainingUnit == null)
            return;

        // 동일한 유닛 ID를 가진 유닛이 1마리 있는 다른 타일을 찾음
        UnitTile matchingTile = controller.UnitTiles.FirstOrDefault(tile =>
            tile != targetTile &&
            tile.UnitCount == 1 &&
            tile.SpawnUnits[0].DataHandler.Data.UnitRank <= EUnitRank.Legendary &&
            tile.SpawnUnits[0].Id == remainingUnit.Id
        );

        if (matchingTile != null)
        {
            // Debug.Log(matchingTile.SpawnUnits[0].DataHandler.Data.ID);
            // Debug.Log(remainingUnit.DataHandler.Data.ID);
            // 1마리 남아있는 유닛을 현재 타일로 옮겨줌
            matchingTile.MoveSingleUnitToOriginalTile(targetTile);
            targetTile.UnitCount++;
            controller.CallUnBlockSummonBtn();
        }
        else
        {
            // 유닛을 해당 타일 가운데 위치로 이동
            remainingUnit.StateMachine.CallUnitMove(targetTile.transform.position);
        }
    }

    public void ShuffleUnitTiles()
    {
        List<UnitTile> tiles = controller.UnitTiles;
        List<UnitTile> spawnedTiles = tiles.FindAll(obj => obj.UnitCount > 0);

        foreach (var tile in spawnedTiles)
        {
            int rand;
            while (true)
            {
                rand = Random.Range(0, tiles.Count);
                if (tiles[rand] != tile)
                    break;
            }

            tile.SwapUnit(tiles[rand]);
        }
    }

    public void SubmergeTileInLava(TileStatusEffectInfo effectInfo, int num)
    {
        List<UnitTile> unitTiles = controller.UnitTiles;

        DeActiveTileInLava();
        eventTileHash.Clear();
        
        eventTileHash = CollectionUtils.GetUniqueCollections(unitTiles, num);
        
        foreach (UnitTile tile in eventTileHash)
        {
            tile.SetStatutEffectUnitTile(effectInfo);
        }
    }

    public void DeActiveTileInLava()
    {
        foreach (UnitTile tile in eventTileHash)
        {
            tile.DeActiveEffect(ECCType.Lava);
            if (tile.UnitCount == 1) // 합쳐질 유닛이 있다면 유닛 합치기
            {
                ReconstructSpawnUnit(tile);
            }
        }
    }

    public void DeActiveTileInFreeze()
    {
        List<UnitTile> freezeTileList = controller.UnitTiles.FindAll(tile => tile.IsFreeze);
        foreach (UnitTile tile in freezeTileList)
        {
            tile.DeActiveEffect(ECCType.Freeze);
            if (tile.UnitCount == 1) // 합쳐질 유닛이 있다면 유닛 합치기
            {
                ReconstructSpawnUnit(tile);
            }
        }
    }
}
