using System.Collections;
using UnityEngine;

public class WaterTornado : ActiveSkill
{
    [SerializeField] private float attackDuration = 2f;
    [SerializeField] private float ticDelay = 0.1f;

    private Effect effect;
    private WaitForSeconds ticDelayTime;
    private Coroutine attackCoroutine;

    private void Init()
    {
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
        ActiveEffect();
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(ActiveSkillRoutine());
    }

    private IEnumerator ActiveSkillRoutine()
    {
        float curTime = 0;
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.WaterTornado);

        while (curTime < attackDuration)
        {
            AllLineHit();
            curTime += ticDelay;
            yield return ticDelayTime;
        }
        EndSkill();
    }

    private void ActiveEffect()
    {
        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_WaterTornado).ReturnMyComponent<Effect>();
    }

    public override void EndSkill()
    {
        StopCoroutine(attackCoroutine);
        base.EndSkill();
    }
}