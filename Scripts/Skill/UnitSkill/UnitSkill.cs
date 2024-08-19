using System.Collections.Generic;
using UnityEngine;

public class UnitSkill : Skill
{
    [Header("Only For Unit Skill")]
    [SerializeField][Range(0.5f, 10f)] protected float damageRate;
    protected Unit unit;
    protected List<Enemy> targetEnemies;
    protected Enemy targetEnemy;
    protected bool isTargetOn; // true일 경우 사용 가능, 다만 범위 스킬일 경우 그냥 시전 가능

    private float damage;
    public void Init(Unit unit)
    {
        this.unit = unit;
        targetEnemies = new List<Enemy>();
        isTargetOn = true;
    }

    #region UnitSkill

    #region UnitSkill_Single
    protected void SingleHitByHealth(SpecialAttack specialAttackData)
    {
        damage = unit.StatHandler.CurrentStat.AttackPower * damageRate;
        if (targetEnemy == null)
            CreateSingleTargetListByHealth();

        if (targetEnemy == null)
        {
            isTargetOn = false;
            return; 
        }

        isTargetOn = true;
        SingleAttack(damage, specialAttackData);
    }

    protected void SingleHitByHealth()
    {
        damage = unit.StatHandler.CurrentStat.AttackPower * damageRate;
        if (targetEnemy == null)
            CreateSingleTargetListByHealth();

        if (targetEnemy == null) 
        {
            isTargetOn = false;
            return; 
        }

        isTargetOn = true;
        SingleAttack(damage);
    }

    private void CreateSingleTargetListByHealth()
    {
        int idx = 0;
        int maxHealth = 0;

        if (GameManager.Instance.System.Mission.HuntEnemy.gameObject.activeSelf)
        {
            targetEnemy = GameManager.Instance.System.Mission.HuntEnemy;
            return;
        }


        while (idx < GameManager.Instance.Stage.SpawnEnemyList.Count)
        {
            // 예외 처리
            if (GameManager.Instance.Stage.SpawnEnemyList[idx] == null)
            {
                idx++;
                continue;
            }

            if (GameManager.Instance.Stage.SpawnEnemyList[idx].HealthSystem.GetCurHealth() > maxHealth)
            {
                targetEnemy = GameManager.Instance.Stage.SpawnEnemyList[idx];
            }
            idx++;
        }
    }
    #endregion

    #region UnitSkill_Splash
    protected void SplashHit(float splashRange)
    {
        damage = unit.StatHandler.CurrentStat.AttackPower * damageRate;
        CreateSplashTargetList(splashRange);
        MultiAttack(damage);
    }

    protected void SplashHit(float splashRange, SpecialAttack specialAttackData)
    {
        damage = unit.StatHandler.CurrentStat.AttackPower * damageRate;
        CreateSplashTargetList(splashRange);
        MultiAttack(damage, specialAttackData);
    }

    private void CreateSplashTargetList(float splashRange)
    {
        // EnemyList에서 splash범위 내에 있는 enemy들 모두 TargetList에 추가
        int idx = 0;
        float splashDistance = splashRange;
        float sqrDist;
        float sqrThre;

        targetEnemies.Clear();

        Enemy huntEnemy = GameManager.Instance.System.Mission.HuntEnemy;
        if (huntEnemy.gameObject.activeSelf)
        {
            sqrDist = Vector3.SqrMagnitude(transform.position - huntEnemy.transform.position);
            sqrThre = splashDistance * splashDistance;

            if (sqrDist < sqrThre)
            {
                targetEnemies.Add(huntEnemy);
            }
        }

        while (idx < GameManager.Instance.Stage.SpawnEnemyList.Count)
        {
            sqrDist = Vector3.SqrMagnitude(transform.position - GameManager.Instance.Stage.SpawnEnemyList[idx].transform.position);
            sqrThre = splashDistance * splashDistance;

            if (sqrDist < sqrThre)
            {
                targetEnemies.Add(GameManager.Instance.Stage.SpawnEnemyList[idx]);
            }
            idx++;
        }
    }
    #endregion

    #region UnitSkill_Line
    protected void LineHit(int targetIdx)
    {
        damage = unit.StatHandler.CurrentStat.AttackPower * damageRate;
        CreateLineTargetList(targetIdx);
        MultiAttack(damage);
    }

    private void CreateLineTargetList(int targetIdx)
    {
        // EnemyList에서 splash범위 내에 있는 enemy들 모두 TargetList에 추가
        int idx = 0;
        int enemyCurIdx;
        targetEnemies.Clear();

        Enemy huntEnemy = GameManager.Instance.System.Mission.HuntEnemy;
        if (huntEnemy.gameObject.activeSelf)
        {
            if (huntEnemy.TargetIndex == targetIdx)
            {
                targetEnemies.Add(huntEnemy);
            }
        }

        while (idx < GameManager.Instance.Stage.SpawnEnemyList.Count)
        {
            enemyCurIdx = GameManager.Instance.Stage.SpawnEnemyList[idx].TargetIndex;
            if (enemyCurIdx == targetIdx) targetEnemies.Add(GameManager.Instance.Stage.SpawnEnemyList[idx]);
            idx++;
        }
    }
    #endregion

    #region UnitSkill_AllLine
    protected void AllLineHit()
    {
        damage = unit.StatHandler.CurrentStat.AttackPower * damageRate;
        CreateAllLineTargetList();
        MultiAttack(damage);
    }

    private void CreateAllLineTargetList()
    {
        // EnemyList에서 splash범위 내에 있는 enemy들 모두 TargetList에 추가
        int idx = 0;
        targetEnemies.Clear();

        Enemy huntEnemy = GameManager.Instance.System.Mission.HuntEnemy;
        if (huntEnemy.gameObject.activeSelf)
        {
            targetEnemies.Add(huntEnemy);
        }

        while (idx < GameManager.Instance.Stage.SpawnEnemyList.Count)
        {
            targetEnemies.Add(GameManager.Instance.Stage.SpawnEnemyList[idx]);
            idx++;
        }
    }
    #endregion

    #region AttackType
    private void MultiAttack(float damage)
    {
        foreach (Enemy enemy in targetEnemies)
        {
            if (enemy.HealthSystem.IsDie()) continue;
            enemy.HealthSystem.TakeDamage(damage);
        }
        targetEnemies.Clear();
    }

    private void MultiAttack(float damage, SpecialAttack specialAttackData)
    {
        foreach (Enemy enemy in targetEnemies)
        {
            if (enemy.HealthSystem.IsDie()) continue;
            enemy.HealthSystem.TakeDamage(damage);
            enemy.StatusHandler.ApplyStatusEffect(ESpecialAttackType.Stun, specialAttackData, enemy);
        }
        targetEnemies.Clear();
    }

    private void SingleAttack(float damage)
    {
        targetEnemy.HealthSystem.TakeDamage(damage);
    }

    private void SingleAttack(float damage, SpecialAttack specialAttackData)
    {
        targetEnemy.HealthSystem.TakeDamage(damage);
        targetEnemy.StatusHandler.ApplyStatusEffect(ESpecialAttackType.Stun, specialAttackData, targetEnemy);
    }
    #endregion

    #endregion
}