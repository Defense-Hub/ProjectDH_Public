using System;
using DG.Tweening;
using TransitionsPlus;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public enum ETutorialOrder { Gold, Summon, Stage, Mission, Gacha, Myth, Reinforce }

public class TutorialUIController : MonoBehaviour
{
    [field: Header("# HUD Info")]
    [field: SerializeField] public RectTransform[] UserInterfaces { get; private set; }

    [field: Header("# TouchBlock Info")]
    [field: SerializeField] public GameObject TouchArea { get; private set; }
    
    [field: Header("# Cursor Event Info")]
    [field: SerializeField] public CursorEventHandler Cursor { get; private set; }
    
    public DialogueEventHandler DialogueEvent { get; private set; }
    [SerializeField] private UIHighlightEventHandler highlightEvent;
    private TutorialEvent tutorialEvent;
    [field: SerializeField] public bool IsEvent { get; set; }
    [field: SerializeField] public bool IsTouch { get; set; }
    
    private void Awake()
    {
        DialogueEvent = GetComponentInChildren<DialogueEventHandler>(true); 
        highlightEvent.Controller = this;
        DialogueEvent.Controller = this;
        
    }

    public void TutorialInit()
    {
        DialogueEvent.SetDialougueData();

        for (int i = 0; i < UserInterfaces.Length; i++)
        {
            UserInterfaces[i].gameObject.SetActive(false);
        }
        
        tutorialEvent = new TutorialEvent(this);

    }
    
    // 터치 이벤트
    public void TouchCallBack()
    {
        if(!IsTouch)
            return;
        
        IsTouch = false;
        DOTween.Kill("Arrow");
        
        if (IsEvent)
        {
            // 행동 이벤트
            DialogueEvent.gameObject.SetActive(false);
            tutorialEvent.OnTutorialEvent();
        }
        else
        {
            // 대화 이벤트
            DialogueEvent.StartDialouge();
        }
    }

    public void HighlightEventOn(ETutorialOrder orderType, Action callback)
    {
        int order = (int)orderType;
        UserInterfaces[order].gameObject.SetActive(true);
        highlightEvent.SetHighlightPosition(UserInterfaces[order], callback);
    }
}