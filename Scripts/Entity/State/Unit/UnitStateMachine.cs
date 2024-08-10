using System;
using UnityEngine;

public class UnitStateMachine : EntityStateMachine
{
    public Vector3? TargetPos { get; private set; }

    // State 변환 이벤트 생성
    // 어떤 State에 존재하든 이동 event가 들어오면 특정 위치로 이동
    public event Action<Vector3> OnMove;

    public Unit Unit { get; private set; }
    
    public CCInfo curCCInfo = new CCInfo();

    // State 선언
    public UnitIdleState IdleState { get; private set; }
    public UnitMoveState MoveState { get; private set; }
    public UnitAttackState AttackState { get; private set; }
    public UnitHardCCState HardCCState { get; private set; }
    public UnitSkillState SkillState { get; private set; }

    // StateMachine 생성자
    
    public UnitStateMachine(Unit unit)
    {
        Entity = unit;
        Unit = Entity as Unit;

        IdleState = new UnitIdleState(this);
        MoveState = new UnitMoveState(this);
        AttackState = new UnitAttackState(this);
        HardCCState = new UnitHardCCState(this);
        SkillState = new UnitSkillState(this);
        OnMove += ChangeMoveState;
    }

    // 함수
    public bool IsMoving()
    {
        return currentState == MoveState;
    }

    private void ChangeMoveState(Vector3 targetPos)
    {
        TargetPos = targetPos;
        ChangeState(MoveState);
    }

    public void InitTargetPos()
    {
        TargetPos = null;
    }

    public void CallUnitMove(Vector3 targetPos)
    {
        if(!Unit.StatusHandler.IsHardCC)
            OnMove?.Invoke(targetPos);
    }

    // TODO : HardCC State 변경 함수 ( 어떤 CC인지? ) 
    public void ChangeHardCCState(CCInfo ccInfo)
    {
        if (Unit.StateMachine.currentState == MoveState) return;
        Unit.StatusHandler.IsHardCC = true;
        // curCCInfo가 비어있거나, 더 높은 우선순위의 CCInfo가 들어왔을 때 ccInfo 변경
        if (curCCInfo.duration == 0 || ((int)ccInfo.ccType > (int)curCCInfo.ccType))
        {
            curCCInfo = ccInfo;
            ChangeState(HardCCState);
        }
        // curCCInfo의 ccType과 새로들어온 ccInfo의 ccType이 같을 경우 duration만 갱신
        else if(curCCInfo.ccType == ccInfo.ccType)
        {
            curCCInfo.duration = ccInfo.duration;
        }
    }

    // 강제로 IdleState로 변경해주는 함수
    public void ForceIdleChange()
    {
        ChangeState(IdleState);
    }
}

