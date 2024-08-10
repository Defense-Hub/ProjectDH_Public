using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EntityStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(entityStateMachine.Entity.AnimationData.IdleParameterHash);
        Rotate((enemy.WayPoints[enemy.TargetIndex].transform.position - enemy.transform.position).normalized);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(entityStateMachine.Entity.AnimationData.IdleParameterHash);
    }
}
