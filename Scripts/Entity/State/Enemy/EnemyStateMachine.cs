using System.Diagnostics;
using Unity.IO.LowLevel.Unsafe;

public class EnemyStateMachine : EntityStateMachine
{
    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemyHardCCState HardCCState { get; private set; }
    public BossSkillState SkillState { get; private set; }

    public CCInfo curCCInfo = new CCInfo();

    public EnemyStateMachine(Enemy enemy)
    {
        Entity = enemy;

        IdleState = new EnemyIdleState(this);
        MoveState = new EnemyMoveState(this);
        HardCCState = new EnemyHardCCState(this);
        SkillState = new BossSkillState(this);
    }

    // HardCC State 변경 함수
    public void ChangeHardCCState(CCInfo ccInfo)
    {
        //더 높은 우선순위의 CCInfo가 들어왔을 때 ccInfo 변경
        if (((int)ccInfo.ccType > (int)curCCInfo.ccType))
        {
            curCCInfo = ccInfo;
            ChangeState(HardCCState);
        }
        // curCCInfo의 ccType과 새로들어온 ccInfo의 ccType이 같을 경우 duration만 갱신
        else if (curCCInfo.ccType == ccInfo.ccType)
        {
            curCCInfo.duration = ccInfo.duration;
        }
    }
}
