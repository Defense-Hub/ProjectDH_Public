using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HuntTimer : MonoBehaviour
{
    [SerializeField] private float initialTime = 60f; // 타이머 초기값 설정
    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime;

    private Coroutine timerCoroutine;

    private void Start()
    {
        // 타이머 초기화
        currentTime = initialTime;
    }

    private IEnumerator TimerCoroutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 프레임당 시간 감소
            currentTime = Mathf.Clamp(currentTime, 0, initialTime); // 현재 시간이 음수로 가지 않도록 보장

            // 타이머를 UI에 표시
            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
                timerText.text = FormatTime(currentTime);
            }

            yield return null; // 다음 프레임까지 대기
        }
        OnTimerEnd();
    }

    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        currentTime = initialTime; // 타이머 초기화
        timerCoroutine = StartCoroutine(TimerCoroutine()); // 타이머 시작
    }

    private void OnTimerEnd()  // 타이머가 끝났을 때 호출
    {
        timerText.gameObject?.SetActive(false);

        Debug.Log("타이머가 종료되었습니다!");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); // 분 계산
        int seconds = Mathf.FloorToInt(time % 60); // 초 계산
        return $"{minutes:00}:{seconds:00}"; // 포맷팅: "MM:SS"
    }
}
