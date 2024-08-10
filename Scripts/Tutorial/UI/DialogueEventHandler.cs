using System;
using System.Collections;
using System.Collections.Generic;
using GameDataTable;
using TMPro;
using UnityEngine;
using KoreanTyper;
using DG.Tweening;
using UnityEngine.Serialization;

public class DialogueEventHandler : MonoBehaviour
{
    [Header("# Dialogue Info")]
    [SerializeField] private List<DialogueData> tutorialDialougeList;
    [SerializeField] private GameObject dialougePanel;
    [SerializeField] private float typingTime;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private int curIndex;
    
    [Header("# Dialogue Arrow Info")]
    [SerializeField] private RectTransform dialougeArrow;
    [SerializeField] private float arrowMovePosY;
    [SerializeField] private float arrowMoveDuration;
    [SerializeField] private float arrowShowTime;
    private Vector3 arrowInitVec;
    
    private WaitForSeconds typingWait;
    private WaitForSeconds showArrowWait;
    private Coroutine typingCoroutine;
    
    public TutorialUIController Controller { get; set; }
    private void Awake()
    {
        typingWait = new WaitForSeconds(typingTime);
        showArrowWait = new WaitForSeconds(arrowShowTime);
        arrowInitVec = dialougeArrow.position;
    }

    public void SetDialougueData()
    {
        List<TutorialDialogueDataSheet> sheetDatas = TutorialDialogueDataSheet.GetList();
        tutorialDialougeList = new List<DialogueData>();

        for (int i = 0; i < sheetDatas.Count; i++)
        {
            if (tutorialDialougeList.Count == sheetDatas[i].DialogueOrder)
            {
                tutorialDialougeList.Add(new DialogueData());
            }

            tutorialDialougeList[tutorialDialougeList.Count - 1].dialogueList.Add(new Dialouge(sheetDatas[i].Dialogue, sheetDatas[i].isEvent));
        }
        
    }

    public void StartDialouge()
    {
        if (tutorialDialougeList[curIndex] == null)
        {
            Debug.LogError($"{curIndex} 번째 대화가 존재하지 않습니다.");
            return;
        }

        Dialouge dialouge = tutorialDialougeList[curIndex].GetDialogue();
        if(dialouge == null)
        {
            Debug.LogError($"{curIndex} 번째 대화에 남아있는 텍스트가 없습니다.");
            return;
        }

        if (tutorialDialougeList[curIndex].DialogueComplete())
        {
            curIndex++;
        }

        SetDialogueObject(false);
        
        gameObject.SetActive(true);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        typingCoroutine = StartCoroutine(TypingDialogueCoroutine(dialouge));

    }

    private IEnumerator TypingDialogueCoroutine(Dialouge dialogue)
    {
        int dialogueLength = dialogue.dialogueText.GetTypingLength();

        for (int i = 0; i <= dialogueLength; i++)
        {
            dialogueText.text = dialogue.dialogueText.Typing(i);
            yield return typingWait;
        }
        
        yield return showArrowWait;

        Controller.IsEvent = dialogue.isEvent;
        
        SetDialogueObject(true);
        dialougeArrow.position = arrowInitVec;
        dialougeArrow.DOAnchorPosY(arrowMovePosY, arrowMoveDuration).SetLoops(-1,LoopType.Yoyo).SetId("Arrow");
    }

    private void SetDialogueObject(bool isActivate)
    {
        if(!Controller.TouchArea.activeSelf)
            Controller.TouchArea.SetActive(true);
        
        Controller.IsTouch = isActivate;
        dialougeArrow.gameObject.SetActive(isActivate);
    }
}
