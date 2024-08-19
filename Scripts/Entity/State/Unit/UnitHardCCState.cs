using UnityEngine;

public class UnitHardCCState : UnitBaseState
{
    private Effect effect;
    public UnitHardCCState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        base.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        // 효과 적용
        // TODO : type에 맞는 이펙트 On
        StartAnimation(stateMachine.Unit.AnimationData.IdleParameterHash);
        base.Enter();
        ActivateEffect();
    }

    public override void Exit()
    {
        // 효과 적용 해제
        // TODO : 위에서 켜둔 Effect Off
        stateMachine.curCCInfo.ccType = ECCType.Default;
        StopAnimation(stateMachine.Unit.AnimationData.IdleParameterHash);
        stateMachine.curCCInfo.callBack?.Invoke();
        DeActivateEffect();
        stateMachine.Unit.StatusHandler.IsHardCC = false;
        base.Exit();
    }

    public override void Update()
    {
        // 타이머 돌리기
        base.Update();
        stateMachine.curCCInfo.duration -= Time.deltaTime;
        
        // 멈추기 전까진 다른 State전환 막아버리기
        if(stateMachine.curCCInfo.duration < 0)
        {
            stateMachine.curCCInfo.duration = 0;
            if(stateMachine.curCCInfo.ccType != ECCType.Freeze && stateMachine.curCCInfo.ccType != ECCType.Lava)
                // 이펙트 비활성화.
                effect.gameObject.SetActive(false);

            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private void ActivateEffect()
    {
        switch(stateMachine.curCCInfo.ccType)
        {
            case ECCType.Stun:
                effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EntityStun).ReturnMyComponent<Effect>();
                // 이펙트 크기 & 스케일 지정
                effect.SetEffect(stateMachine.Unit.EffectWayPoint.localScale, stateMachine.Unit.EffectWayPoint.position);
                break;
            case ECCType.Freeze:
                effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EntityFreeze).ReturnMyComponent<Effect>();
                effect.SetEffect(stateMachine.Unit.EffectWayPoint.localScale, stateMachine.Unit.EffectWayPoint.position);
                break;
            case ECCType.Lava:
                effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_EntityLava).ReturnMyComponent<Effect>();
                effect.SetEffect(stateMachine.Unit.EffectWayPoint.localScale, stateMachine.Unit.EffectWayPoint.position);
                break;
        }
    }

    private void DeActivateEffect()
    {
        effect.gameObject.SetActive(false);
    }
}
