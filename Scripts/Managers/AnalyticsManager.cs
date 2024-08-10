using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class AnalyticsManager : Singleton<AnalyticsManager>
{
    private string development = "development";
    private string production = "production";

    protected async override void Awake()
    {
        base.Awake();
        try
        {
#if UNITY_EDITOR
            var options = new InitializationOptions();
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN
            var options = new InitializationOptions().SetEnvironmentName(production);
#endif
            await UnityServices.InitializeAsync(options);

            // todo 동의 UI 띄우기
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Analytics Start");
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public void AnalyticsStageEnd()
    {
        int curStage = GameManager.Instance.Stage.CurStageLevel;
        bool isStageClear = GameManager.Instance.Stage.IsStageClear;
        //int killCount;
        int curGold = PlayerDataManager.Instance.inGameData.Gold;
        int curGemstone = PlayerDataManager.Instance.inGameData.GemStone;
        //int curGemStone;
        //int summonCount;
        Dictionary<string,object> parameters = new Dictionary<string, object>
        {
            { "curStage", curStage },
            { "IsStageClear", isStageClear }
        };
        AnalyticsService.Instance.CustomData("DEV_StageEnd", parameters);
        Debug.Log("SendEvent : StageEnd");
    }

    public void AnalyticsGameOff()
    {
        int curStage = GameManager.Instance.Stage.CurStageLevel;
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "curStage", curStage }
        };
        AnalyticsService.Instance.CustomData("DEV_GameOff", parameters);
        Debug.Log("SendEvent : GameOff");
    }

    public void AnalyticsStageCount()
    {
        int curStage = GameManager.Instance.Stage.CurStageLevel;
        string nickname = PlayerDataManager.Instance.SaveData.Name;
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            {"curStage", curStage},
            {"nickname", nickname}
        };

        AnalyticsService.Instance.CustomData("DEV_StageCount", parameters);
    }

    public void AnalyticsPlayCount()
    {
        string nickname = PlayerDataManager.Instance.SaveData.Name;
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            {"nickname", nickname}
        };

        AnalyticsService.Instance.CustomData("DEV_PlayCount", parameters);
    }
}
