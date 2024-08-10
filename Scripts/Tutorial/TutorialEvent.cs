using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialEvent
{
    public TutorialUIController controller;

    public List<Action> eventList;

    private int eventStep = 0;


    private Button summonBtn;
    private Button gachaBtn;


    public TutorialEvent(TutorialUIController controller)
    {
        this.controller = controller;
        summonBtn = controller.UserInterfaces[(int)ETutorialOrder.Summon].GetComponent<Button>();
        GameManager.Instance.UnitSpawn.Controller.CombinationUI.SetCombinationLock(true);
        gachaBtn = controller.UserInterfaces[(int)ETutorialOrder.Mission].GetComponent<Button>();
        
        eventList = new List<Action>();

        eventList.Add(GoldHighlightEvent);
        eventList.Add(SummonHighlightEvent);
        eventList.Add(DragCursorEvent);
        eventList.Add(StageHighlightEvent);
        eventList.Add(StageStartEvent);
        eventList.Add(CombinationEvent);
        eventList.Add(MissionEvent);
        eventList.Add(GachaHighlightEvent);
        eventList.Add(EpicCombinationHighlightEvent);
        eventList.Add(ReinforceHighlightEvent);
        eventList.Add(TutorialEndEvent);
    }

    public void OnTutorialEvent()
    {
        if (eventList.Count <= eventStep || eventStep < 0)
        {
            Debug.LogError($"{eventStep} 번째 이벤트는 존재하지 않습니다.");
            return;
        }

        eventList[eventStep++]?.Invoke();
    }

    #region Gold Event

    private void GoldHighlightEvent() // 재화 이벤트
    {
        controller.HighlightEventOn(ETutorialOrder.Gold, controller.DialogueEvent.StartDialouge);
    }

    #endregion

    #region Summon Event

    private void SummonHighlightEvent()
    {
        summonBtn.enabled = true;
        
        controller.HighlightEventOn(ETutorialOrder.Summon, SummonEventCallback);
    }

    private void SummonEventCallback()
    {
        summonBtn.interactable = true;
        summonBtn.onClick.AddListener(SummonAfterEvent);
    }

    private void SummonAfterEvent()
    {
        summonBtn.onClick.RemoveListener(SummonAfterEvent);
        controller.DialogueEvent.StartDialouge();
    }

    #endregion

    #region Drag Event

    private void DragCursorEvent()
    {
        controller.TouchArea.SetActive(false);

        UnitTile startTile = GameManager.Instance.UnitSpawn.Controller.UnitTiles[0];
        UnitTile endTile = GameManager.Instance.UnitSpawn.Controller.UnitTiles[2];

        controller.Cursor.DragCursorEvent(startTile, endTile, controller.DialogueEvent.StartDialouge);
    }

    #endregion

    #region Stage Event

    private void StageHighlightEvent()
    {
        controller.HighlightEventOn(ETutorialOrder.Stage, controller.DialogueEvent.StartDialouge);
    }

    private void StageStartEvent()
    {
        controller.TouchArea.SetActive(false);

        PlayerDataManager.Instance.inGameData.ChangeGold(100);
        
        GameManager.Instance.Stage.GameStart();
    }

    #endregion

    #region Combination Evnet

    private void CombinationEvent()
    {
        List<UnitTile> tiles = GameManager.Instance.UnitSpawn.Controller.UnitTiles;

        UnitTile combinationTile = tiles.Find(tile => tile.UnitCount == 2) ?? tiles.Find(tile => tile.UnitCount == 1);

        // 1마리 밖에 없을 땐
        if (combinationTile.UnitCount == 1)
        {
            GameManager.Instance.UnitSpawn.DEV_SpawnUnit(combinationTile.SpawnUnits[0].Id);
        }

        CursorEventHandler cursor = controller.Cursor;
        Vector2 clickCursorPos = cursor.MainCamera.WorldToScreenPoint(combinationTile.transform.position + cursor.OffsetVec);
        
        controller.TouchArea.SetActive(false);
        
        GameManager.Instance.UnitSpawn.Controller.CombinationUI.SetCombinationLock(false);
        
        cursor.ClickCursurEvent(clickCursorPos, true);
    }
    
    #endregion

    #region Mission Event

    private void MissionEvent()
    {
        controller.HighlightEventOn(ETutorialOrder.Mission, controller.DialogueEvent.StartDialouge);
    }

    #endregion
    
    #region Gacha Event

    private void GachaHighlightEvent()
    {
        controller.HighlightEventOn(ETutorialOrder.Gacha, controller.DialogueEvent.StartDialouge);
    }
    
    
    #endregion
    
    #region Epic Combination Event
    
    private void EpicCombinationHighlightEvent()
    {
        controller.HighlightEventOn(ETutorialOrder.Myth, controller.DialogueEvent.StartDialouge);
    }
    
    #endregion
    
    #region Reinforce Event
    
    private void ReinforceHighlightEvent()
    {
        controller.HighlightEventOn(ETutorialOrder.Reinforce, controller.DialogueEvent.StartDialouge);
    }
    
    #endregion
    
    #region Tutorial End Event
    
    private void TutorialEndEvent()
    {
        GameManager.Instance.LoadScene.LoadTutorialToStartScene();
    }
    
    #endregion
}