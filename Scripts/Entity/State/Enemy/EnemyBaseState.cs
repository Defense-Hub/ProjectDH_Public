using UnityEngine;

public class EnemyBaseState : EntityBaseState
{
    protected Enemy enemy;

    public EnemyBaseState(EntityStateMachine stateMachine) : base(stateMachine)
    {
        enemy = stateMachine.Entity as Enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if(enemy.HealthSystem.IsDie())
        {
            Stop();
            return;
        }
    }

    protected void Move(Vector3 moveDir, float moveSpeed)
    {
        enemy.transform.Translate(moveSpeed * Time.deltaTime * moveDir, Space.World);
    }

    protected void Stop()
    {
        enemy.Stat.MoveSpeed = 0f;
    }

    protected void Rotate(Vector3 dir)
    {
        enemy.transform.rotation = Quaternion.LookRotation(dir);
    }

    protected void SetDefaultMoveSpeed()
    {
        enemy.Stat.MoveSpeed = enemy.EnemyData.MoveSpeed;
    }
}
