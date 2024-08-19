using System.Collections;
using UnityEngine;

public class Slow : IStatus
{
    //Coroutine slowCoroutine;
    float slowTime;
    CCInfo slowCCInfo;

    public void Apply(StatusHandler handler, SpecialAttack specialAttackData, Enemy target)
    {
        if (target.StatusHandler.IsHardCC) return;

        slowTime = specialAttackData.Slow.EffectDuration;
        slowCCInfo = new CCInfo
        {
            ccType = ECCType.Slow,
            duration = slowTime
        };

        if (handler.coroutine != null)
            handler.StopRunningCoroutine();
        handler.coroutine = handler.RunCoroutine(ActiveSlow(target, specialAttackData.Slow.Delta));
    }

    private IEnumerator ActiveSlow(Enemy target, float delta)
    {
        target.StatusHandler.IsHardCC = true;
        target.StateMachine.SetCCInfo(slowCCInfo);
        target.StateMachine.HardCCState.ActivateEffect();
        target.Stat.MoveSpeed *= delta;

        yield return new WaitForSeconds(slowTime);

        target.StatusHandler.IsHardCC = false;
        target.StateMachine.SetDefaultCCInfo();
        target.StateMachine.HardCCState.DeActivateEffect();
        target.Stat.MoveSpeed = target.EnemyData.MoveSpeed;
    }
}
