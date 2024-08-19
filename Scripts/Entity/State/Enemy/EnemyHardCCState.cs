using UnityEngine;

public class EnemyHardCCState : EnemyBaseState
{
    public Effect CurCCEffect { get; private set; }
    public EnemyHardCCState(EntityStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Stop();
        ActivateEffect();
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
        enemy.StatusHandler.IsHardCC = false;
        enemy.StateMachine.SetDefaultCCInfo();
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

    public void ActivateEffect()
    {
        DeActivateEffect();
        switch (enemy.StateMachine.curCCInfo.ccType)
        {
            case ECCType.Slow:
                CurCCEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EntitySlow).ReturnMyComponent<Effect>();
                CurCCEffect.transform.parent = enemy.EffectWayPoint;
                break;
            case ECCType.Stun:
                CurCCEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EntityStun).ReturnMyComponent<Effect>();
                break;
        }
        CurCCEffect.SetEffect(enemy.EffectWayPoint.localScale, enemy.EffectWayPoint.position);
    }

    public void DeActivateEffect()
    {
        if (CurCCEffect != null)
        {
            CurCCEffect.gameObject.SetActive(false);
            CurCCEffect.transform.parent = GameManager.Instance.Pool.poolTransforms[1];
            CurCCEffect = null;
        }
    }
}
