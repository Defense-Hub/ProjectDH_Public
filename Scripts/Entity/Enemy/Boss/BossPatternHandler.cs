using System;
using System.Collections;
using UnityEngine;

public class BossPatternHandler : MonoBehaviour
{
    [Header("패턴 시작 전 Idle(무적) 시간")]
    [SerializeField] private float idleTime;

    // 보스 패턴 : 등장, 지속 스킬(쿨타임마다 사용), 지정된 Hp에 도달하면 사용, 죽을 때 
    public event Action OnStartPatternEvent;
    public event Action OnDiePatternEvent;
    public event Action<float> OnHealthChangePatternEvent;

    private Boss boss;
    private HealthSystem healthSystem;
    private WaitForSeconds idleTimeSeconds;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        idleTimeSeconds = new WaitForSeconds(idleTime);
    }

    public void Init(Boss boss)
    {
        this.boss = boss;
    }

    private void CallStartPatternEvent()
    {
        OnStartPatternEvent?.Invoke();
    }

    public void CallDiePatternEvent()
    {
        OnDiePatternEvent?.Invoke();
    }

    public void BossPattern()
    {
        StartCoroutine(StartPattern());
    }

    private IEnumerator StartPattern()
    {
        boss.StateMachine.ChangeState(boss.StateMachine.IdleState);
        boss.HealthSystem.ChangeInvincibility(true);

        yield return idleTimeSeconds;
        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.HealthSystem.ChangeInvincibility(false);
        CallStartPatternEvent();
        StartCoroutine(CoolTimeSkill());
    }

    private IEnumerator CoolTimeSkill()
    {
        // 쿨타임 스킬이 존재하지 않을 때
        if (boss.SkillHandler.IsEmptyCoolTimeSkillList()) yield break;
        // 첫 스킬 딜레이
        yield return boss.SkillHandler.firSkillDelay;
        // 살아있는 동안 반복해서 스킬 시전
        while (!healthSystem.IsDie())
        {
            // 스킬 시전
            boss.SkillHandler.UseCoolTimeSkill();
            // 스킬 시전이 끝날 때까지 대기
            while (boss.SkillHandler.IsCasting)
            {
                yield return null;
            }
            // 쿨타임 스킬이 여러 개일 경우 스킬 시전 텀 필요 
            yield return boss.SkillHandler.skillTermValue;
        }
    }

    public void CheckHealth(float healthPercentage)
    {
        if (healthSystem.IsDie()) return;
        OnHealthChangePatternEvent?.Invoke(healthPercentage);
        return;
    }
}