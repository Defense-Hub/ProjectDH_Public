using System.Collections;
using UnityEngine;

public class Slow : IStatus
{
    Coroutine slowCoroutine;
    float slowTime;
    public void Apply(StatusHandler handler, SpecialAttack specialAttackData, Enemy target)
    {
        if (target.StatusHandler.IsHardCC) return;

        slowTime = specialAttackData.Slow.EffectDuration;
        if (slowCoroutine != null)
            handler.StopRunningCoroutine(slowCoroutine);
        SlowData slowData = specialAttackData.Slow;
        slowCoroutine = handler.RunCoroutine(ActiveSlow(target, slowData));
    }

    private IEnumerator ActiveSlow(Enemy target, SlowData slowData)
    {
        target.StatusHandler.IsHardCC = true;
        target.Stat.MoveSpeed *= slowData.Delta;
        yield return new WaitForSeconds(slowTime);
        target.StatusHandler.IsHardCC = false;
        target.Stat.MoveSpeed = target.EnemyData.MoveSpeed;
    }
}
