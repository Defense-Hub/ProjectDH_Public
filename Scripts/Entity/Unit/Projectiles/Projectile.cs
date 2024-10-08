using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    private List<Enemy> targetList;
    private SpecialAttack specialAttack;
    private EAttackType attackType;
    private ESpecialAttackType specialAttackType;
    // specialAttack 지속 시간, 확률 
    private float attackPower;
    private float projectileMoveSpeed;
    private Coroutine ActiveProjectile;


    [SerializeField] private float disableRange = 0.2f;
    [SerializeField] private float projectileMoveSpeedOrigin = 10f;
    [SerializeField] private MeshRenderer meshRenderer;


    private void Awake()
    {
        targetList = new List<Enemy>();  
    }

    public void Init(UnitStat stat, Enemy target, int id)
    {
        // 내부 초기화
        meshRenderer.enabled = true;
        projectileMoveSpeed = projectileMoveSpeedOrigin;

        // 타겟 설정
        targetList.Add(target);

        // 스텟 설정
        attackPower = stat.AttackPower;
        specialAttack = stat.SpecialAttack;
        SetSpecialAttackType(stat.SpecialAttack);

        // 렌더러 설정
        SetProjectileRenderer(id);

        if(stat.AttackType == EAttackType.Melee)
        {
            meshRenderer.enabled = false;
            projectileMoveSpeed *= 10f;
        }

        // 발사
        if (ActiveProjectile != null) 
        {
            StopCoroutine(ActiveProjectile);
        }
        ActiveProjectile = StartCoroutine(MoveProjectile());
    }

    private IEnumerator MoveProjectile()
    {
        if (targetList.Count == 0 || targetList[0] == null)
        {
            DisableObject();
            gameObject.SetActive(false);
            yield break;
        }

        Vector3 targetPos = targetList[0].transform.position;
        float sqrDistance = Vector3.SqrMagnitude(transform.position - targetPos);
        float sqrThreshold = disableRange * disableRange;
        while(sqrDistance > sqrThreshold)
        {
            sqrDistance = Vector3.SqrMagnitude(transform.position - targetPos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, projectileMoveSpeed * Time.deltaTime);
            yield return null;
        }

        // Splash일 경우 splash 범위 만큼의 target을 모두 targetList에 add
        if(specialAttackType.HasFlag(ESpecialAttackType.Splash))
        {
            CreateSplashTargetList();
        }

        SoundManager.Instance.PlayInGameSfx(EInGameSfx.BaseAttack);

        // enemy의 상태가 바뀐다
        // splash가 아닌 경우 최초 add된 target에만 공격이 적용
        foreach (Enemy target in targetList)
        {
            if (target == null) continue;
            target.StatusHandler.ApplyStatusEffect(specialAttackType, specialAttack, target);
            target.HealthSystem.TakeDamage(attackPower);
        }

        DisableObject();
        gameObject.SetActive(false);
    }

    private void CreateSplashTargetList()
    {
        // EnemyList에서 splash범위 내에 있는 enemy들 모두 TargetList에 추가
        int idx = 0;
        float splashDistance = specialAttack.Splash.SplashRange;
        float sqrDist;
        float sqrThre;
        
        Enemy huntEnemy = GameManager.Instance.System.Mission.HuntEnemy;
        if (huntEnemy.gameObject.activeSelf)
        {
            sqrDist = Vector3.SqrMagnitude(transform.position - huntEnemy.transform.position);
            sqrThre = splashDistance * splashDistance;

            if (sqrDist < sqrThre)
            {
                targetList.Add(huntEnemy);
            }
        }
        
        while(idx < GameManager.Instance.Stage.SpawnEnemyList.Count)
        {
            sqrDist = Vector3.SqrMagnitude(transform.position - GameManager.Instance.Stage.SpawnEnemyList[idx].transform.position);
            sqrThre = splashDistance * splashDistance;

            if (sqrDist < sqrThre)
            {
                targetList.Add(GameManager.Instance.Stage.SpawnEnemyList[idx]);
            }
            idx++;
        }
    }

    private void SetSpecialAttackType(SpecialAttack specialAttack)
    {
        // 초기화
        specialAttackType = ESpecialAttackType.Default;
        // TODO : List 순회와 같은 방법으로 SpecialAttack의 종류가 확장되더라도 바로 적용 가능하게 수정
        if (RandomEvent.GetBoolRandomResult(specialAttack.Splash.Probabillity))
        {
            specialAttackType |= ESpecialAttackType.Splash;
        }

        if (RandomEvent.GetBoolRandomResult(specialAttack.Stun.Probabillity))
        {
            specialAttackType |= ESpecialAttackType.Stun;
        }

        if (RandomEvent.GetBoolRandomResult(specialAttack.Slow.Probabillity))
        {
            specialAttackType |= ESpecialAttackType.Slow;
        }
    }

    private void SetProjectileRenderer(int id)
    {
        switch(id % 100)
        {
            case (int)EElemental.Fire:
                meshRenderer.material = GameDataManager.Instance.FireProjectileMaterial;
                break;
            case (int)EElemental.Ice:
                meshRenderer.material = GameDataManager.Instance.IceProjectileMaterial;
                break;
            case (int)EElemental.Dark:
                meshRenderer.material = GameDataManager.Instance.DarkProjectileMaterial;
                break;
            case (int)EElemental.Ground:
                meshRenderer.material = GameDataManager.Instance.GroundProjectileMaterial;
                break;
            case (int)EElemental.Water:
                meshRenderer.material = GameDataManager.Instance.WaterProjectileMaterial;
                break;
            default:
                meshRenderer.material = GameDataManager.Instance.FireProjectileMaterial;
                break;
        }
    }

    private void DisableObject()
    {
        targetList.Clear();
        if (ActiveProjectile != null)
        {
            StopCoroutine(ActiveProjectile);
        }
    }
}