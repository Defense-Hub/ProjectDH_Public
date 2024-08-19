using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    public GamblingSystem Gamble { get; private set; }
    public CombinationSystem Combination { get; private set; }
    public SellSystem Sell { get; private set; }
    public MissionSystem Mission { get; private set; }
    public EnforceSystem Enforce { get; private set; }

    private Coroutine huntMissionDelayCoroutine;
    private void Start()
    {
        GameManager.Instance.System = this;
    }
    

    public void SetGameSystem()
    {
        Gamble = new GamblingSystem();
        Combination = new CombinationSystem(this);
        Sell = new SellSystem();
        Mission = new MissionSystem(this);
        Enforce = new EnforceSystem();
    }

    public int SelectGamble(int gambleType)
    {
        if (!GameManager.Instance.UnitSpawn.Controller.IsAvailableSpawn())
        {
            Debug.LogWarning("Gacha => Summon Tile is Full");
            return (int)EGachaResult.NoSpace;
        }

        int requiredCurrency = GameDataManager.Instance.GambleProbabilityDatas[gambleType].RequiredGemStone;
        if (!PlayerDataManager.Instance.inGameData.HasGemStone(requiredCurrency))
        {
            Debug.LogWarning("Gacha short of Money");
            return (int)EGachaResult.NoMoney;
        }

        PlayerDataManager.Instance.inGameData.ChangeGemStone(-requiredCurrency);

        return Gamble.StartGroupGamble(gambleType);
    }
    
    public void TryCombinationAdvancedUnit (int targetID) // 에픽 조합
    {
        Combination.CombinationAdvancedUnit(targetID);
    }

    [ContextMenu("sell")]
    public void DEV_SellUnit()
    {
        Sell.SellUnit(0); 
    }

    #region 강화
    public bool HasEnoughBasicGold()
    {
        if (!PlayerDataManager.Instance.inGameData.HasGold(Enforce.GetBasicEnforceRequiredCurrency) )
            return false;
        return true;
    }

    public bool HasEnoughAdvancedGem()
    {
        if (!PlayerDataManager.Instance.inGameData.HasGemStone(Enforce.GetAdvanceEnforceRequiredCurrency))
            return false;
        return true;
    }

    public void TryEnforceBasicUnit()
    {
        int currency = Enforce.GetBasicEnforceRequiredCurrency;

        if (!HasEnoughBasicGold())
            return;

        PlayerDataManager.Instance.inGameData.ChangeGold(-currency);

        if (PlayerDataManager.Instance.inGameData.BasicUnitEnhancementLevelUp())
        {
            Enforce.ApplBasicAdvancedUnit();
        }
    }

    public void TryEnforceAdvancedUnit()
    {
        int currency = Enforce.GetAdvanceEnforceRequiredCurrency;

        if (!HasEnoughAdvancedGem())
            return;
        
        PlayerDataManager.Instance.inGameData.ChangeGemStone(-currency);
        
        if (PlayerDataManager.Instance.inGameData.AdvancedUnitEnhancementLevelUp())
        {
            Enforce.ApplyEnforceAdvancedUnit();
        }
    }

    #endregion
    public void InitMaterialDictionary(Dictionary<int, List<MaterialInfo>> materialDic, int idx, List<int> materialList)
    {
        if (!materialDic.TryAdd(idx, new List<MaterialInfo>()))
        {
            Debug.LogError($"{idx} => 중복된 Key 값의 데이터 입니다.");
            return;
        }

        foreach (var material in materialList)
        {
            materialDic[idx].Add(new MaterialInfo(material));
        }
    }

    public void CheckMaterialDictionary(List<MaterialInfo> infoList, int id,  bool isCheck)
    {
        foreach (var material in infoList)
        {
            if (material.targetID == id && ((!material.hasUnit && isCheck) || (material.hasUnit && !isCheck)))
            {
                material.hasUnit = isCheck;
                return;
            }
        }
    }

    [ContextMenu("hunt")]
    public void Hunt()
    {
        Mission.TryHuntMission();
    }


    [ContextMenu("GameOver")]
    public async void GameOver()
    {
        Debug.Log("게임 오버");
        UI_GameOver ui_GameOver = await UIManager.Instance.ShowPopupUI<UI_GameOver>(EUIRCode.UI_GameOver);
        ui_GameOver.Init();
    }

    [ContextMenu("GameClear")]
    public async void GameClear()
    {
        Debug.Log("게임 클리어");
        UI_GameClear ui_GameClear = await UIManager.Instance.ShowPopupUI<UI_GameClear>(EUIRCode.UI_GameClear);
        ui_GameClear.Init();
    }

    public void SetGameSpeed(float speedMultiplier)
    {
        Time.timeScale = speedMultiplier;
    }

    public void SetHuntMissionDelay()
    {
        if (huntMissionDelayCoroutine != null)
        {
            StopCoroutine(huntMissionDelayCoroutine);
        }

        huntMissionDelayCoroutine = StartCoroutine(HuntMissionDelayCoroutine());
    }

    private IEnumerator HuntMissionDelayCoroutine()
    {
        yield return Mission.wait;

        SetHuntMissionUI();
    }

    public async void SetHuntMissionUI()
    {
        await UIManager.Instance.ShowPopupUI<UI_Hunt>(EUIRCode.UI_Hunt);
    }
}