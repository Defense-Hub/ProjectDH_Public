using UnityEngine;
using UnityEngine.UIElements;

public class BossSkillState : EnemyBaseState
{
    Boss boss;
    public BossSkillState(EntityStateMachine stateMachine) : base(stateMachine)
    {
        boss = enemy as Boss;
    }

    public override void Enter()
    {
        base.Enter();
        BeginSkill(boss.SkillHandler.SkillInfo);
    }

    public override void Exit()
    {
        base.Exit();
        SetDefaultMoveSpeed();
        StopAnimation(boss.SkillHandler.SkillInfo.animParameterHash);
    }

    private void BeginSkill(SkillInfo beginSkillInfo)
    {
        Stop();
        boss.SkillHandler.CheckSkillEnd();
        beginSkillInfo.onSkill?.Invoke();
    }
}
