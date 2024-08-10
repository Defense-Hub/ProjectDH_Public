using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : PassiveSkill
{
    [Header("# Skill Data")]
    [SerializeField] private float explodeDelay;
    [SerializeField] private float explodeRangeRate;
    //[SerializeField] private float explodeDamageRate;

    [Header("# Skill Effect")]
    [SerializeField] private FireExplosionEffect delayEffect;
    [SerializeField] private FireExplosionEffect explodeEffect;

    private float explodeDefaultRange = 2f;
    private float delayDefaultRange = 4f;

    private WaitForSeconds explodeDelayTime;
    private WaitForSeconds explodeEffectTime;
    private Coroutine explodeCoroutine;
    private Vector3 targetPos;
    private List<Enemy> enemies;

    private void Awake()
    {
        explodeDelayTime = new WaitForSeconds(explodeDelay);
        explodeEffectTime = new WaitForSeconds(1f);

        enemies = new List<Enemy>();
    }

    private void Start()
    {
        delayEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_ExRangeIndicator).ReturnMyComponent<FireExplosionEffect>();
        explodeEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_FireExplosion).ReturnMyComponent<FireExplosionEffect>();

        delayEffect.transform.localScale = Vector3.one * delayDefaultRange * explodeRangeRate;
        explodeEffect.transform.localScale = Vector3.one * explodeRangeRate;

        delayEffect.gameObject.SetActive(false);
        explodeEffect.gameObject.SetActive(false);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        // 위치(타격 중인 몬스터가 있는 라인) 잡고
        targetPos = unit.TargetEnemy.transform.position;
        // active TargetPoint 생성
        transform.position = targetPos + Vector3.up;
        delayEffect.transform.position = targetPos + Vector3.up;
        explodeEffect.transform.position = targetPos + Vector3.up;

        gameObject.SetActive(true);
        Init();
    }

    private void Init()
    {
        if(explodeCoroutine != null)
        {
            StopCoroutine(explodeCoroutine);
        }
        explodeCoroutine = StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        delayEffect.gameObject.SetActive(true);
        yield return explodeDelayTime;
        yield return explodeEffectTime;
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.FireExplosion);
        explodeEffect.gameObject.SetActive(true);
        SplashHit((explodeDefaultRange * explodeRangeRate));
        yield return explodeEffectTime;
        explodeEffect.gameObject.SetActive(false);
        delayEffect.gameObject.SetActive(false);

        EndSkill();
    }

    public override void EndSkill()
    {
        base.EndSkill();
        // state 전환
        unit.StateMachine.ForceIdleChange();
        // 이 오브젝트 off
        gameObject.SetActive(false);
    }
}