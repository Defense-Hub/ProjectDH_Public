using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem
{
    private GameSystemManager system;
    public bool[] MissionClearArr { get; private set; }
    
    public Dictionary<int,List<MaterialInfo>> MissionDict { get; private set; }
    
    public Enemy HuntEnemy { get; private set; }
    private readonly float huntTime = 60f;
    public WaitForSeconds wait;
    private Timer huntTimer;
    private int huntLevel = 1;
    public MissionSystem(GameSystemManager system)
    {
        this.system = system;
        
        MissionClearArr = new bool[GameDataManager.Instance.MissionDatas.Count];
        
        MissionDict = new Dictionary<int, List<MaterialInfo>>();
        
        foreach (var mission in GameDataManager.Instance.MissionDatas)
        {
            this.system.InitMaterialDictionary(MissionDict, mission.missionIndex, mission.materialIDs);
        }

        wait = new WaitForSeconds(huntTime);
        
        LoadHuntEnemy();
    }

    private async void LoadHuntEnemy()
    {
        GameObject obj = await AddressableManager.Instance.InstantiateAsync(EAddressableType.HuntEnemy, 0);
        HuntEnemy = obj.GetComponent<Enemy>();
        huntTimer = HuntEnemy.GetComponentInChildren<Timer>();
        obj.SetActive(false);
        HuntEnemy.transform.parent = system.transform;
    }
    
    public void CheckUnitMission(int id , bool isCheck) // 미션 재료 유닛 체크
    {
        
        foreach (var mission in MissionDict)
        {
            if(MissionClearArr[mission.Key])
                continue;
            
            system.CheckMaterialDictionary(MissionDict[mission.Key], id, isCheck);
        }
    
        if(isCheck)
            IsMissionClear();
        
    }
    
    
    private void IsMissionClear() // 미션 클리어 확인
    {
        foreach (var mission in MissionDict)
        {
            if(MissionClearArr[mission.Key])
                continue;

            List<MaterialInfo> infoList = MissionDict[mission.Key]; ;
            for (int i = 0; i < infoList.Count; i++)
            {
                if (!infoList[i].hasUnit)
                {
                    break;
                }

                if (i == infoList.Count - 1)
                {
                    MissionClearArr[mission.Key] = true;
                    GetMissionReword(mission.Key);
                    Debug.Log($"{mission.Key}번 미션 성공 !");
                }
            }
        }
    }

    private void GetMissionReword(int index) 
    {
        // 보상
        PlayerDataManager.Instance.inGameData.ChangeGold(GameDataManager.Instance.MissionDatas[index].goldReward);
        PlayerDataManager.Instance.inGameData.ChangeGemStone(GameDataManager.Instance.MissionDatas[index].gemStoneReward);
    }

    public void TryHuntMission()
    {
        if (HuntEnemy.gameObject.activeSelf)
        {
            Debug.LogError("사냥 몬스터가 이미 소환 중 입니다 !");
            return;
        }
        
        HuntEnemy.gameObject.SetActive(true);
        huntTimer.StartTimer(huntTime, (() => { HuntEnemy.HealthSystem.CallDieEvent();}));
        HuntEnemy.transform.position = GameManager.Instance.Stage.EnemyWayPoints[0].position;
        HuntEnemy.EnemyInit(huntLevel);
    }

    public void DieHuntEnemy()
    {
        PlayerDataManager.Instance.inGameData.ChangeGemStone(GameDataManager.Instance.HuntEnemyDatas[huntLevel].GemStoneReward);
        if (GameDataManager.Instance.HuntEnemyDatas.ContainsKey(huntLevel + 1))
        {
            huntLevel++;
        }
    }
}
