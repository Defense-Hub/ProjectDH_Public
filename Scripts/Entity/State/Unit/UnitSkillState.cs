public class UnitSkillState : UnitBaseState
{
    public UnitSkillState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        base.stateMachine = stateMachine;
    }

    // Todo : Skill별 애니매이션 제작 및 실행
    public override void Enter()
    {
        base.Enter();
        // TODO : Skill이 여러개일 때 사용하는 skill의 animation에 대응되도록 수정
        StartAnimation(stateMachine.Unit.AnimationData.Skill1ParameterHash);
        stateMachine.Unit.AnimationEventHandler.OnSkillEnd += EndSkill;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Unit.AnimationData.Skill1ParameterHash);
        stateMachine.Unit.AnimationEventHandler.OnSkillEnd -= EndSkill;
    }

    private void EndSkill()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
