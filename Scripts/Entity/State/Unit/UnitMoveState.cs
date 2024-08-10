using UnityEngine;

public class UnitMoveState : UnitBaseState
{
    float moveDiff = 0.1f;
    Vector3 unitPos, targetPos;
    public UnitMoveState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        base.stateMachine = stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Entity.AnimationData.MoveParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Entity.AnimationData.MoveParameterHash);
    }

    public override void Update() 
    {
        base.Update();
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (stateMachine.TargetPos == null)
            stateMachine.ChangeState(stateMachine.IdleState);

        Rotate();
        MoveToward();
    }

    private void UpdatePosition()
    {
        unitPos = stateMachine.Unit.transform.position;
        targetPos = (Vector3)stateMachine.TargetPos;
        // TODO : 이동하는 중에 TargetPos가 없어지는 경우

        //unitPos.y = 0f;
        //targetPos.y = 0f;
    }

    private Vector3 GetTargetDirection()
    {
        UpdatePosition();
        Vector3 dir;
        dir = (unitPos - targetPos).normalized;
        return dir;
    }

    private void Rotate()
    {
        Vector3 direction = GetTargetDirection();
        if (direction != Vector3.zero)
        {
            Transform unitTransform = stateMachine.Unit.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            unitTransform.localRotation = Quaternion.Slerp(unitTransform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void MoveToward()
    {
        if (stateMachine.TargetPos == null){
            // TargetPos가 사라진 경우 Move를 그만두고 Idle로 돌아가는 로직 추가.
            stateMachine.ChangeState(stateMachine.IdleState);
            return; 
        }
        UpdatePosition();
        Unit curUnit = stateMachine.Unit;
        stateMachine.Unit.transform.position = Vector3.MoveTowards(unitPos, targetPos, curUnit.StatHandler.CurrentStat.MoveSpeed * Time.deltaTime);

        float sqrDistance = Vector3.SqrMagnitude(unitPos - targetPos);
        float sqrThreshold = moveDiff * moveDiff;
        if (sqrDistance < sqrThreshold)
        {
            stateMachine.Unit.transform.position = targetPos;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
