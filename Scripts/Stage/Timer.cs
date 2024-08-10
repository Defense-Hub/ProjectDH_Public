using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action<int> OnChangeTimerText;
    public event Action<int> OnEndEvent; 
    private float currentTimer = 0f;

    private Coroutine timerCoroutine;
    private WaitForSeconds wait;
    private void Awake()
    {
        wait = new WaitForSeconds(1f);
    }

    public void StartTimer(float duration, Action callback)
    {
        // Debug.Log($"StartTimer: {duration}");
        currentTimer = duration;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(RunTimer(callback));
    }

    private IEnumerator RunTimer(Action callback)
    {
        OnChangeTimerText?.Invoke((int)currentTimer);
        while (currentTimer > 0)
        {
            yield return wait;
            currentTimer -= 1;
            // 타이머 텍스트 ?
            OnChangeTimerText?.Invoke((int)currentTimer);
        }
        OnEndEvent?.Invoke(GameManager.Instance.Stage.CurStageLevel);
        callback?.Invoke();
    }
    
}

//한 스테이지가 끝나고 재정비 시간
//