using UnityEngine;

public class UnitBaseState : EntityBaseState
{
    protected UnitStateMachine stateMachine;
    public UnitBaseState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}