using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CursorEventHandler : MonoBehaviour
{
    [Header("# Cursur Drag Event Info")]
    [SerializeField] private float dragTime = 1f;
    [field:SerializeField] public Vector3 OffsetVec { get; private set; }

    [Header("# Cursur Click Event Info")]
    [SerializeField] private Color clickColor;
    [SerializeField] private float clickTime = 1f;
    [field:SerializeField] public Vector2 ClickCursorOffset { get; private set; }
    private Image cursurImage;
    
    public Camera MainCamera { get; private set; }
    private RectTransform rect;
    private Coroutine cursorCallbackCoroutine;
    private void Awake()
    {
        if (Camera.main != null)
        {
            MainCamera = Camera.main;
        }

        rect = GetComponent<RectTransform>();
        cursurImage = GetComponent<Image>();
    }
    
    public void DragCursorEvent(UnitTile startTile, UnitTile endTile, Action callback)
    {
        gameObject.SetActive(true);
        
        Vector2 screenStartPos = MainCamera.WorldToScreenPoint(startTile.transform.position + OffsetVec);
        Vector2 screenEndPos = MainCamera.WorldToScreenPoint(endTile.transform.position + OffsetVec);
        

        // UI 요소의 위치를 시작 위치로 설정
        rect.anchoredPosition = screenStartPos;

        // RectTransform을 이용해 시작 위치에서 끝 위치로 이동하고 반복
        rect.DOAnchorPos(screenEndPos, dragTime).SetLoops(-1, LoopType.Restart).SetAutoKill(true).SetId("Drag");

        if (cursorCallbackCoroutine != null)
        {
            StopCoroutine(cursorCallbackCoroutine);
        }

        cursorCallbackCoroutine = StartCoroutine(DragCallbackCoroutine(startTile, callback));
    }

    private IEnumerator DragCallbackCoroutine(UnitTile tile, Action callback)
    {
        while (tile.SpawnUnits[0] != null)
        {
            yield return null;
        }
        
        gameObject.SetActive(false);
        DOTween.Kill("Drag");
        callback?.Invoke();
    }

    public void ClickCursurEvent(Vector2 cursurPos, bool isCombination)
    {
        gameObject.SetActive(true);
        
        rect.position = cursurPos + ClickCursorOffset;


        cursurImage.DOColor(clickColor, clickTime).SetLoops(-1, LoopType.Yoyo).SetAutoKill(true).SetId("Click");


        if (isCombination)
        {
            if (cursorCallbackCoroutine != null)
            {
                StopCoroutine(cursorCallbackCoroutine);
            }

            cursorCallbackCoroutine = StartCoroutine(ClickCallbackCoroutine());   
        }
    }

    private IEnumerator ClickCallbackCoroutine()
    {
        RectTransform combination = GameManager.Instance.UnitSpawn.Controller.CombinationUI.Rect;
        while (!combination.gameObject.activeSelf)
        {
            yield return null;
        }

        ClickDGKill();
    }

    public void ClickDGKill()
    {
        DOTween.Kill("Click");
        gameObject.SetActive(false);
    }
}