using UnityEngine;

public class GamblingSystem
{
    // public void StartTotalGamble()
    // {
    //     int rand = RandomEvent.GetArrRandomResult(totalGamblePercent);
    //
    //     if (rand <= 0)
    //     {
    //         Debug.Log("통합 도박 실패 ㅜ !");
    //         return;
    //     }
    //         
    //     GameManager.Instance.UnitSpawn.SpawnRankUnit(rand - 1);
    //
    //     Debug.Log("통합 도박 성공 !");
    // }
    
    public int StartGroupGamble(int GambleStep)
    {
        if (!RandomEvent.GetBoolRandomResult(GameDataManager.Instance.GambleProbabilityDatas[GambleStep].GambleProbability))
        {
            Debug.Log("그룹 도박 실패 ㅜ !");
            return (int)EGachaResult.Fail;
        }

        // 유닛을 바로 뽑는게 아니라 id만 뽑고 return
        // fail일 땐 -1 return
        int rand = Random.Range(GameDataManager.Instance.UnitMinMaxID[GambleStep + 1].minMaxIDs[0],
                                GameDataManager.Instance.UnitMinMaxID[GambleStep + 1].minMaxIDs[1]);

        return rand;
    }
}
