using UnityEngine;

public class UnitAttackState : UnitBaseState
{
    public UnitAttackState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        base.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Unit.AnimationEventHandler.OnCheck += CheckTarget;
        stateMachine.Unit.AnimationEventHandler.OnAttack += Attack;
        StartAnimation(stateMachine.Unit.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Unit.AnimationData.AttackParameterHash);
        stateMachine.Unit.AnimationEventHandler.OnCheck -= CheckTarget;
        stateMachine.Unit.AnimationEventHandler.OnAttack -= Attack;
    }

    public override void Update()
    {
        base.Update();
        Rotate();
    }

    private void CheckTarget()
    {
        // 타겟이 죽은 경우
        if(stateMachine.Unit.TargetEnemy.HealthSystem.IsDie()) 
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // 타겟이 범위 밖으로 나간 경우
        Vector3 TargetPosition = stateMachine.Unit.TargetEnemy.transform.position;
        Vector3 UnitPosition = stateMachine.Unit.transform.position;
        float sqrDistance = Vector3.SqrMagnitude(UnitPosition - TargetPosition);
        float sqrAttackRange = stateMachine.Unit.StatHandler.CurrentStat.AttackRange * stateMachine.Unit.StatHandler.CurrentStat.AttackRange;
        if (sqrDistance > sqrAttackRange)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return; 
        }
    }

    private void Attack()
    {
        // TODO : Unit별 Projectile 변경
        if (stateMachine.Unit.SkillHandler != null)
        {
            if (stateMachine.Unit.SkillHandler.HasActivableSkill())
            {
                stateMachine.ChangeState(stateMachine.SkillState);
                return;
            }
        }
        Projectile projectile = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_Projectile).ReturnMyComponent<Projectile>();
        projectile.transform.position = stateMachine.Unit.transform.position;
        projectile.Init(stateMachine.Unit.StatHandler.CurrentStat, stateMachine.Unit.TargetEnemy, stateMachine.Unit.Id);
    }

    private Vector3 GetTargetDirection()
    {
        Vector3 dir;
        if(stateMachine.Unit.TargetEnemy != null)
            dir = (stateMachine.Unit.transform.position - stateMachine.Unit.TargetEnemy.transform.position).normalized;
        else
            dir = stateMachine.Unit.transform.forward;
        return dir;
    }

    private void Rotate()
    {
        Vector3 direction = GetTargetDirection();
        if (direction != Vector3.zero)
        {
            Transform unitTransform = stateMachine.Unit.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            unitTransform.rotation = Quaternion.Slerp(unitTransform.rotation, targetRotation, 3f * Time.deltaTime);
        }
    }
}
