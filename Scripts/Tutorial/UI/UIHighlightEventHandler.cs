using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHighlightEventHandler : MonoBehaviour
{
    [Header("# HighLight Info")]
    [SerializeField] private float highlightTime;
    [SerializeField] private float highlightInitScale;


    private readonly float highlightTimeOffset = 1f;
    private RectTransform rect;
    private Coroutine highlightCoroutine;
    private WaitForSeconds highlightWait;
    public TutorialUIController Controller { get; set; }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        highlightWait = new WaitForSeconds(highlightTime + highlightTimeOffset);
    }

    public void SetHighlightPosition(RectTransform targetRect, Action callback)
    {
        SetHighlightObject(true);
        
        rect.sizeDelta = targetRect.sizeDelta;
        rect.position = targetRect.position;
        
        rect.localScale = Vector3.one * highlightInitScale;
        
        if (highlightCoroutine != null)
        {
            StopCoroutine(highlightCoroutine);
        }

        highlightCoroutine = StartCoroutine(HighLightCoroutine(callback));
    }

    private IEnumerator HighLightCoroutine(Action callback)
    {
        rect.DOScale(1, highlightTime).SetAutoKill(true);
        
        yield return highlightWait;
        
        // 콜백
        callback?.Invoke();
            
        SetHighlightObject(false);
    }

    private void SetHighlightObject(bool isActivate)
    {
        gameObject.SetActive(isActivate);
    }
}
