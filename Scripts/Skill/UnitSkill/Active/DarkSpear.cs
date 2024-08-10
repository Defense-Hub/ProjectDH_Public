using System;
using System.Collections;
using UnityEngine;

public class DarkSpear : ActiveSkill
{
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDuration = 2f;
    [SerializeField] private float ticDelay = 0.1f;
    
    private Effect effect;
    private WaitForSeconds attackDelayTime;
    private WaitForSeconds ticDelayTime;
    private Coroutine attackCoroutine;
    private void Init()
    {
        attackDelayTime = new WaitForSeconds(attackDelay);
        ticDelayTime = new WaitForSeconds(ticDelay);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        Init();
        ActiveSkill();
    }

    private void ActiveSkill()
    {
        InitEffect();
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.DarkSpear);

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(ActiveSkillRoutine());
    }

    private void InitEffect()
    {
        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_DarkSpear).ReturnMyComponent<Effect>();
        effect.transform.position = unit.transform.position + Vector3.up;
    }

    private IEnumerator ActiveSkillRoutine()
    {
        // 1초간 대기
        yield return attackDelayTime;

        GameManager.Instance.CameraEvent.CameraShakeEffect(attackDuration);

        float curTime = 0f;
        while(curTime < attackDuration)
        {
            if(targetEnemy != null) 
                effect.transform.LookAt(targetEnemy.transform.position);
            SingleHitByHealth();
            curTime += ticDelay;
            yield return ticDelayTime;
        }

        EndSkill();
    }

    public override void EndSkill()
    {
        StopCoroutine(attackCoroutine);
        base.EndSkill();
    }
}
