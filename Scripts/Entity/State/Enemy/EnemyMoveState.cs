using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    private readonly float range = 0.01f;

    public EnemyMoveState(EntityStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(entityStateMachine.Entity.AnimationData.MoveParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(entityStateMachine.Entity.AnimationData.MoveParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(IsArrivalToWayPoint()) 
        {
            // 현재 enemy 위치 웨이포인트로 변경
            enemy.transform.position = enemy.WayPoints[enemy.TargetIndex].transform.position;
            // 다음 웨이포인트로 목적지 변경
            enemy.SetTargetIndex((enemy.TargetIndex+1) % enemy.WayPoints.Length);
        }
        else if (!enemy.HealthSystem.IsDie())
        {
            MoveToWayPoint();
        }
    }

    private void MoveToWayPoint()
    {
        Vector3 moveDir = GetDirectionToWayPoint();
        Rotate(moveDir);
        Move(moveDir, enemy.Stat.MoveSpeed);
    }

    private Vector3 GetDirectionToWayPoint()
    {
        Vector3 dir = (enemy.WayPoints[enemy.TargetIndex].transform.position - enemy.transform.position ).normalized;
        dir.y = 0;
        return dir;
    }

    private bool IsArrivalToWayPoint()
    {
        Vector3 distanceVector = (enemy.transform.position - enemy.WayPoints[enemy.TargetIndex].transform.position);
        float distance = distanceVector.sqrMagnitude;
        return distance < range;
    }
}
