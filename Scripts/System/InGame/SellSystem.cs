using System;

public class SellSystem
{

    public int GetSellCurrency(EUnitRank unitRank)
    {
        return unitRank switch
        {
            EUnitRank.Common => (PlayerDataManager.Instance.inGameData.UnitSpawnRequriedGold / 10) * 2,
            EUnitRank.Rare => 1,
            EUnitRank.Unique => 2,
            EUnitRank.Legendary => 3,
            _ => 0
        };
    }
    public void SellUnit(int tileNum)
    {
        UnitTile tile = GameManager.Instance.UnitSpawn.Controller.UnitTiles[tileNum];

        EUnitRank unitRank = tile.SpawnUnits[0].DataHandler.Data.UnitRank;
        if (tile.UnitCount == 0 || unitRank >= EUnitRank.Epic)
            return;

        // 마지막 유닛을 비활성화하고 참조 값을 null로 설정
        tile.DeActivateUnit();
        
        // Todo : 재화 획득 로직 추가

        int currency = GetSellCurrency(unitRank);
        if (unitRank == EUnitRank.Common)
        {
            PlayerDataManager.Instance.inGameData.ChangeGold(currency);
        }
        else
        {
            PlayerDataManager.Instance.inGameData.ChangeGemStone(currency);
        }
        
    }
}