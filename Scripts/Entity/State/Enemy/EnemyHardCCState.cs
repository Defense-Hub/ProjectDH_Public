using UnityEngine;

public class EnemyHardCCState : EnemyBaseState
{
    private Effect effect;
    public EnemyHardCCState(EntityStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Stop();
        // TODO :: 추후 EnemyHardCCState에 Stun이 아닌 다른 스탯 적용시 구조 수정 
        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EntityStun).ReturnMyComponent<Effect>();
        effect.SetEffect(enemy.EffectWayPoint.localScale, enemy.EffectWayPoint.position);
    }

    public override void Exit()
    {
        base.Exit();
        TerminateHardCC();
    }

    public override void Update()
    {
        base.Update();
        CheckDuration();
        if (enemy.HealthSystem.IsDie())
        {
            DeActivateEffect();
        }
    }

    private void TerminateHardCC()
    {
        SetDefaultMoveSpeed();
        DeActivateEffect();

        enemy.StateMachine.curCCInfo.callBack?.Invoke();
        enemy.StateMachine.curCCInfo.ccType = ECCType.Default;
    }
    
    private void CheckDuration()
    {
        enemy.StateMachine.curCCInfo.duration -= Time.deltaTime;
        if (enemy.StateMachine.curCCInfo.duration <= 0)
        {
            enemy.StateMachine.curCCInfo.duration = 0;
            enemy.StateMachine.ChangeState(enemy.StateMachine.MoveState);
        }
    }

    private void DeActivateEffect()
    {
        if (effect != null)
        {
            effect.gameObject.SetActive(false);
            effect = null;
        }
    }
}
