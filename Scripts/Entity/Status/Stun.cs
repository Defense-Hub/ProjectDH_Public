public class Stun : IStatus
{
    float stunTime;
    CCInfo stunCCInfo;

    public void Apply(StatusHandler handler, SpecialAttack specialAttack, Enemy target)
    {
        // 시전 중에는 스턴 걸리지 않게 하기
        if(target.TryGetComponent<Boss>(out Boss boss))
        {
            if(boss.SkillHandler.IsCasting)  return;
        }
        stunTime = specialAttack.Stun.EffectDuration * (1 - target.Stat.Toughness / 100.0f);

        stunCCInfo = new CCInfo
        {
            ccType = ECCType.Stun,
            duration = stunTime
        };

        target.StateMachine.ChangeHardCCState(stunCCInfo);
    }
}
