using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SplitSkill : OneTimeSkill
{
    [Header("# Split Skill")]
    [SerializeField] private int splitEnemyCount;
    [SerializeField] private float moveDelay;

    [Header("# Split Skill Effect")]
    [SerializeField] private GameObject splitEffect;
    [SerializeField] private TextMeshProUGUI splitTXT;

    private Coroutine splitCoroutine;
    private WaitForSeconds moveDelayWaitForSeconds;
    private readonly List<Boss_Water> splitBossList = new List<Boss_Water>();
    private Boss_Water waterBoss;

    private void Start()
    {
        if(waterBoss == null)
            waterBoss = boss as Boss_Water;
        if(moveDelayWaitForSeconds == null)
            moveDelayWaitForSeconds = new WaitForSeconds(moveDelay);
    }

    public override void UseSkill()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.SplitSkill);
        waterBoss.StateMachine.ChangeState(waterBoss.StateMachine.SkillState);
        if(splitCoroutine!= null) 
            StopCoroutine(splitCoroutine);
        splitCoroutine = StartCoroutine(SpawnSplitEnemy());
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (splitCoroutine != null)
            StopCoroutine(splitCoroutine);

        // 분열 스킬을 사용한 보스가 MoveDelay 도중 죽었을 경우 예외처리
        foreach (Boss_Water splitBoss in splitBossList)
        {
            if (splitBoss != null)
                splitBoss.StateMachine.ChangeState(splitBoss.StateMachine.MoveState);
        }
        DisableTXT(splitTXT);
    }

    private IEnumerator SpawnSplitEnemy()
    {
        InitTXT(splitTXT, transform.position + Vector3.up * 4f, 1f, "분열!", moveDelay);
        splitEffect.SetActive(true);

        splitBossList.Clear();
        splitBossList.Add(waterBoss); 

        waterBoss.Split();
        waterBoss.HealthSystem.InitHealthSystem(waterBoss.HealthSystem.GetCurHealth());

        for (int i = 0; i < splitEnemyCount - 1; i++)
        {
            Boss_Water splitBoss = GameManager.Instance.Pool.SpawnFromPool((int)EEnemyType.Boss).ReturnMyComponent<Boss_Water>();
            if (splitBoss != null)
            {
                GameManager.Instance.Stage.SpawnEnemyList.Add(splitBoss);
                splitBossList.Add(splitBoss);

                // 생성 위치 설정
                splitBoss.transform.position = transform.position + transform.forward * -0.4f * (i + 1);
                // 크기 설정
                splitBoss.transform.localScale = waterBoss.transform.localScale;
                //목표 웨이포인트 - 기준 보스의 웨이포인트
                splitBoss.SetTargetIndex(waterBoss.TargetIndex);
                // 체력 설정 - 기준 보스의 현재 체력
                splitBoss.HealthSystem.InitHealthSystem(waterBoss.HealthSystem.GetCurHealth());
            }
        }

        // 분열된 보스 초기화
        foreach (Boss_Water splitBoss in splitBossList)
        {
            if (splitBoss != null)
            {
                splitBoss.SplitBossInit(waterBoss.EnemyData, waterBoss.Stat, waterBoss.CurSplitCount, waterBoss.ui_CountDown);
                // 분열 중일 때 무적
                splitBoss.HealthSystem.ChangeInvincibility(true);
            } 
        }

        yield return moveDelayWaitForSeconds;

        // 분열된 보스 움직이기
        foreach (Boss_Water splitBoss in splitBossList)
        {
            if(splitBoss != null)
            {
                splitBoss.StateMachine.ChangeState(splitBoss.StateMachine.MoveState);
                splitBoss.HealthSystem.ChangeInvincibility(false);
            }
        }

        DisableTXT(splitTXT);
        splitEffect.SetActive(false);
        waterBoss.SkillHandler.FinishSkillCast();
    }
}