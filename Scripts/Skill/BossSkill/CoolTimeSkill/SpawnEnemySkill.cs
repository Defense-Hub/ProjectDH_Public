using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnEnemySkill : CoolTimeSkill
{
    [Header("# Spawn Enemy Skill")]
    [SerializeField] private int SpawnEnemyCount;
    [SerializeField] private float SpawnInterval;

    [Header("# Spawn Enemy Skill Effect")]
    [SerializeField] private TextMeshProUGUI spawnTXT;

    private Vector3 spawnPos;
    private WaitForSeconds spawnIntervalWaitForSeconds;
    private readonly List<Effect> delayEffects = new List<Effect>();
    private Coroutine spawnEnemyCoroutine;
    private Enemy curSpawnEnemy;

    private void Start()
    {
        spawnIntervalWaitForSeconds = new WaitForSeconds(SpawnInterval);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (IsReadyToUse() && !isStun)
        {
            isReadyToUseSkill = false;
            boss.SkillHandler.SetSkillInfo(
                ESkillType.CoolTime,
                0.2f,
                boss.AnimationData.Skill2ParameterHash,
                () => {
                    if (spawnEnemyCoroutine != null)
                        StopCoroutine(spawnEnemyCoroutine);

                    spawnEnemyCoroutine = StartCoroutine(SpawnEnemy()); }
                );
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (spawnEnemyCoroutine != null)
            StopCoroutine(spawnEnemyCoroutine);

        // 스킬 시전 중 죽었을 때 예외처리
        foreach (Effect effect in delayEffects)
        {
            if (effect != null)
                effect.gameObject.SetActive(false);
        }

        // 소환 중 보스가 죽었을 경우 소환 중이던 몬스터 MoveState로 바꿔주기
        if (curSpawnEnemy != null)
            curSpawnEnemy.StateMachine.ChangeState(curSpawnEnemy.StateMachine.MoveState);

        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
        DisableTXT(spawnTXT);
    }

    private IEnumerator SpawnEnemy()
    {
        UIManager.Instance.UI_Interface.ActivateBossSkillIndicator($"보스가 몬스터를 소환합니다");

        for (int i = 0; i < SpawnEnemyCount; i++)
        {
            Enemy enemy = GameManager.Instance.Pool.SpawnFromPool((int)EEnemyType.Basic).ReturnMyComponent<Enemy>();
            GameManager.Instance.Stage.SpawnEnemyList.Add(enemy);
            UIManager.Instance.UI_EnemyHpBar.SetHpBarToNormalEnemy(enemy);
            curSpawnEnemy = enemy;

            // 불 일반 몬스터 체력 보스 체력과 같지 않도록 StageLevel 조정
            enemy.EnemyInit(GameManager.Instance.Stage.CurStageLevel - 1);
            enemy.StateMachine.ChangeState(enemy.StateMachine.IdleState);

            // 랜덤 스폰 위치 할당
            spawnPos = GetRandomSpawnPosition(enemy);
            enemy.transform.position = spawnPos;

            // 딜레이 이펙트 활성화
            Effect spawnEnemyDelayEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EnemySpawnDelay).ReturnMyComponent<Effect>();
            delayEffects.Add(spawnEnemyDelayEffect);    
            spawnEnemyDelayEffect.transform.position = spawnPos;

            SoundManager.Instance.PlayInGameSfx(EInGameSfx.SpawnEnemySkill);

            // 소환 TXT 활성화
            InitTXT(spawnTXT, transform.position + Vector3.up * 3f, 1f, "소환!", SpawnInterval);

            yield return spawnIntervalWaitForSeconds;

            // 소환 TXT 비활성화
            DisableTXT(spawnTXT);

            enemy.gameObject.SetActive(true);
            enemy.StateMachine.ChangeState(enemy.StateMachine.MoveState);
            curSpawnEnemy = null;
            spawnEnemyDelayEffect.gameObject.SetActive(false);
            delayEffects.Remove(spawnEnemyDelayEffect);
        }

        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.SkillHandler.FinishSkillCast();
        StartCoroutine(SkillCoolTime());
    }

    private Vector3 GetRandomSpawnPosition(Enemy enemy)
    {
        int randomIndex = Random.Range(0, boss.WayPoints.Length);

        Vector3 wp1 = boss.WayPoints[randomIndex].position;
        Vector3 wp2 = boss.WayPoints[(randomIndex + 1) % boss.WayPoints.Length].position;

        float randomValue = Random.Range(0.1f, 0.9f);

        enemy.SetTargetIndex((randomIndex + 1) % boss.WayPoints.Length);
        enemy.transform.LookAt(boss.WayPoints[(randomIndex + 1) % boss.WayPoints.Length].position);

        return Vector3.Lerp(wp1, wp2, randomValue);
    }
}
