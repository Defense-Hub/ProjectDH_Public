using TMPro;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UI_LoadScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadTXT;
    [SerializeField] private TextMeshProUGUI tipTXT;

    private readonly string baseText = "로딩 중";
    private readonly float delay = 2.5f;
    private readonly int dotCount = 3;

    private void Start()
    {
        LoadingAnimation();
        SetRandomTipTXT();
    }

    private void OnDisable()
    {
        DOTween.KillAll(loadTXT);
    }

    private void LoadingAnimation()
    {
        loadTXT.text = baseText;

        // 애니메이션을 반복적으로 수행
        DOTween.To(() => 0, x =>
        {
            loadTXT.text = baseText + new string('.', x % (dotCount + 1));
        }, dotCount, delay);
        DOVirtual.DelayedCall(2.5f, LoadingAnimation);
    }

    private void SetRandomTipTXT()
    {
        // 타이틀 씬이면 Return
        if (tipTXT == null) return;

        int randIdx = Random.Range(0, GameDataManager.Instance.GameTipDatas.Count);
        tipTXT.text = GameDataManager.Instance.GameTipDatas[randIdx];
    }
}
