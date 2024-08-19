using DG.Tweening;
using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class UI_LavaIndicator : UI_Popup
{
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine indicateCoroutine;
    private readonly WaitForSeconds indicateDuration = new WaitForSeconds(3f);

    private void Start()
    {
        if (indicateCoroutine != null)
            StopCoroutine(indicateCoroutine);
        indicateCoroutine = StartCoroutine(BossIndicator());
    }

    public void EndUI()
    {
        if (indicateCoroutine != null)
            StopCoroutine(indicateCoroutine);

        ClosePopupUI();
    }

    private IEnumerator BossIndicator()
    {
        DotweenBossIndicator();
        yield return indicateDuration;
        EndUI();
    }

    private void DotweenBossIndicator()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutSine);
    }
}
