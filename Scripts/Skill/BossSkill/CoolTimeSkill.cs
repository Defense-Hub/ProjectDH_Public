using System.Collections;
using UnityEngine;

public class CoolTimeSkill : BossSkill
{
    [Header("# 스킬 쿨타임")]
    public float coolTime;
    private WaitForSeconds coolTimeWaitForSeconds;

    [Header("# 멈추는 시간")]
    [SerializeField] protected float moveDelay;
    public WaitForSeconds moveDelayWaitForSeconds;

    protected bool isReadyToUseSkill;
    protected bool isStun;

    protected virtual void Awake()
    {
        moveDelayWaitForSeconds = new WaitForSeconds(moveDelay);
        coolTimeWaitForSeconds = new WaitForSeconds(coolTime);
        isReadyToUseSkill = true;
        isStun = false;
    }

    public override void UseSkill()
    {
        if (boss.StateMachine.curCCInfo.ccType == ECCType.Stun)
        {
            boss.SkillHandler.FinishSkillCast();
            isStun=true;
            return;
        }
        isStun = false;
        base.UseSkill();
    }

    protected IEnumerator SkillCoolTime()
    {
        yield return coolTimeWaitForSeconds;
        isReadyToUseSkill = true;
    }

    public bool IsReadyToUse()
    {
        return isReadyToUseSkill;
    }
}
