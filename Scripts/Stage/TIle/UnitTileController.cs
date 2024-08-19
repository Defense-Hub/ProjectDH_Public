using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class UnitTileController : MonoBehaviour
{
    [field: SerializeField] public List<UnitTile> UnitTiles { get; private set; } = new List<UnitTile>();


    [field: SerializeField] public Unit_AttackRange UnitAttackRange { get; private set; }
    [field: SerializeField] public UI_Combination CombinationUI { get; private set; }
    [field: SerializeField] public UI_Sell UI_Sell { get; private set; }

    public TileEventHandler TileEvents { get; private set; }

    public event Action<bool> OnUnBlockSummonBtn;
    public bool IsFullTile { get; set; }

    private IEnumerator Start()
    {
        GameManager.Instance.MapTransform = transform.parent;
        yield return null;
        GameManager.Instance.UnitSpawn.Controller = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            UnitTile tile = transform.GetChild(i).gameObject.AddComponent<UnitTile>();
            UnitTiles.Add(tile);
            tile.TileNum = i;

            tile.UnitAttackRange = UnitAttackRange;
            tile.UICombination = CombinationUI;
            tile.UISell = UI_Sell;
        }

        TileEvents = new TileEventHandler(this);
    }


    public void SetUnitTile(Unit spawnUnit)
    {
        UnitTiles[GetSpawnTileNumber(spawnUnit)].SetUnit(spawnUnit);

        IsFullTile = !IsAvailableSpawn();
        ;
        if (IsFullTile)
            OnUnBlockSummonBtn?.Invoke(false);
    }

    private int GetSpawnTileNumber(Unit spawnUnit)
    {
        int spawnUnitID = spawnUnit.Id;

        // ??는 앞 탐색에서 찾지 못했을 때, 뒤의 내용을 탐색함
        // 맵에 1개만 소환된 동일 유닛이 있는지 탐색 후, 없다면 빈 자리에 소환
        UnitTile tile = UnitTiles.FirstOrDefault(obj => obj.UnitCount == 1 && obj.SpawnUnits[0].Id == spawnUnitID
                                           && !obj.IsStatusEffect() && spawnUnit.DataHandler.Data.UnitRank <= EUnitRank.Legendary)
                        ?? UnitTiles.FirstOrDefault(obj => obj.UnitCount == 0 && !obj.IsStatusEffect());

        if (tile == null)
        {
            throw new InvalidOperationException("No available Spawn Tile !");
        }

        return tile.TileNum;
    }

    public bool IsAvailableSpawn()
    {
        // 소환할 수 있는 타일이 있는지 반환 -> 
        return UnitTiles.Find(obj => (obj.UnitCount == 0 && !obj.IsStatusEffect()));
    }

    public UnitTile GetAvailableRandomUnitTile() // 유닛이 없는 타일 랜덤 반환
    {
        UnitTile[] tiles = UnitTiles.FindAll(obj => obj.UnitCount == 0).ToArray();

        if (tiles.Length == 0)
        {
            return null;
        }
        else
        {
            return tiles[Random.Range(0, tiles.Length)];
        }
    }

    public UnitTile GetCombinationUnitTile(int targetID)
    {
        return UnitTiles.FirstOrDefault(obj =>
            obj.SpawnUnits[0] != null && obj.SpawnUnits[0].Id == targetID);
    }

    public void CallUnBlockSummonBtn() // 유닛이 가득 차서 소환 버튼이 막혔을 때,  
    {
        if (IsAvailableSpawn())
        {
            IsFullTile = false;
            OnUnBlockSummonBtn?.Invoke(true);   
        }
    }
    
    [ContextMenu("Lava")]
    private void d()
    {
        ActivateLava(5);
    }
    
    public void ActivateLava(float recoveryTime)
    {
        TileStatusEffectInfo effectInfo = new TileStatusEffectInfo()
        {
            eBeginEffect = EEffectRcode.E_LavaBegin,
            eAfterEffect = ECCType.Lava,
            preparationTime = recoveryTime,
            activeDuration = 999f,
        };

        TileEvents.SubmergeTileInLava(effectInfo, 10);
    }

    [ContextMenu("deLa")]
    public void DeActivateLava()
    {
        TileEvents.DeActiveTileInLava();
    }
    
    public void DeActivateFreeze()
    {
        TileEvents.DeActiveTileInFreeze();
    }
}