using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_BossIndicator : UI_Popup
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private string[] descriptions = new string[5];

    private readonly WaitForSeconds indicateDuration = new WaitForSeconds(4f);
    private Coroutine indicateCoroutine;

    enum Texts
    {
        TXT_BossWave,
        TXT_BossType,
        TXT_BossDescription,
        TXT_BossLimitTime
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TMP_Text>(typeof(Texts));
    }

    public void SetUI()
    {
        if(indicateCoroutine != null)
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

        GetText((int)Texts.TXT_BossWave).text = $"WAVE {GetCurWave()}";
        GetText((int)Texts.TXT_BossType).text = $"{GetCurBossType()} 보스 등장!";
        GetText((int)Texts.TXT_BossDescription).text = $"{GetCurBossDesc()}";
        GetText((int)Texts.TXT_BossLimitTime).text = $"제한 시간  {GetCurLimitTime()}";

        yield return indicateDuration;
        EndUI();
    }

    private void DotweenBossIndicator()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutSine);
    }

    private int GetCurWave()
    {
        return GameManager.Instance.Stage.StageData.BossStageLevel;
    }

    private string GetCurBossType()
    {
        return (ResourceManager.Instance.currentEnemyType-1) switch
        {
            (int)EAddressableType.WaterEnemy => "물",
            (int)EAddressableType.FireEnemy => "불",
            (int)EAddressableType.IceEnemy => "얼음",
            (int)EAddressableType.EarthEnemy => "땅",
            (int)EAddressableType.DarkEnemy => "어둠",
             _ => "" // default
        };
    }
    private string GetCurBossDesc()
    {
        return (ResourceManager.Instance.currentEnemyType - 1) switch
        {
            (int)EAddressableType.WaterEnemy => descriptions[0],
            (int)EAddressableType.FireEnemy => descriptions[1],
            (int)EAddressableType.IceEnemy => descriptions[2],
            (int)EAddressableType.EarthEnemy => descriptions[3],
            (int)EAddressableType.DarkEnemy => descriptions[4],
            _ => "" // default
        };
    }

    private string GetCurLimitTime()
    {
        TimeSpan timerSpan = TimeSpan.FromSeconds(GameManager.Instance.Stage.StageData.BossWaveTime);
        return string.Format("{0:D2}:{1:D2}", timerSpan.Minutes, timerSpan.Seconds);
    }
}
