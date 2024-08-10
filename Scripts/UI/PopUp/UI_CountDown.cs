using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class UI_CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownTXT;

    private Coroutine countDownSpawn;
    private readonly WaitForSeconds oneSeconds = new WaitForSeconds(1);

    public void InitUI()
    {
        SetActiveCountDownUI(false);
    }

    public void StartCountDown(int duration, Action onCDEnd= null)
    {
        // 코루틴 사용 전 UI 켜주기
        SetActiveCountDownUI(true);

        if (countDownSpawn != null)
            StopCoroutine(countDownSpawn);

        countDownSpawn = StartCoroutine(CountDown(duration, onCDEnd));
    }

    // 카운트다운 UI 띄우기 & 카운트다운 끝나면 이벤트 Invoke
    private IEnumerator CountDown(int duration, Action onCDEnd = null)
    {
        transform.SetParent(UIManager.Instance.UI_Interface.countDownTransform, false);
        UpdateCountDownTXT(duration);

        while (duration > 0)
        {
            yield return oneSeconds;
            duration--;
            UpdateCountDownTXT(duration);
        }
        SetActiveCountDownUI(false);
        onCDEnd?.Invoke();
    }

    public void DestroyUI()
    {
        if (countDownSpawn != null)
            StopCoroutine(countDownSpawn);
        Destroy(gameObject);
    }

    public void SetActiveCountDownUI(bool b)
    {
        gameObject.SetActive(b);
    }

    private void UpdateCountDownTXT(int second)
    {
        countDownTXT.text = second.ToString();
    }
}
