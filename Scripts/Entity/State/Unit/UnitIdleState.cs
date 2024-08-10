using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    private float timeCount = 0f;
    public UnitIdleState(UnitStateMachine stateMachine) : base(stateMachine)
    {
        base.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Entity.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Entity.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        timeCount += Time.deltaTime;
        if(timeCount > stateMachine.Unit.SearchDelay)
        {
            timeCount = 0f;
            SearchTarget();
        }
    }
    // 범위 내에 타겟이 들어왔는지 체크 -> AttackState로 전환
    // 해당 타겟이 죽거나 범위 밖으로 나갈 때 까지 추적

    private void SearchTarget()
    {
        List<Enemy> enemyList = GameManager.Instance.Stage.SpawnEnemyList;
        float minDistance = float.MaxValue;
        Enemy targetEnemy = null;
        int idx = 0;

        Enemy huntEnemy = GameManager.Instance.System.Mission.HuntEnemy;
        if (huntEnemy.gameObject.activeSelf)
        {
            float sqrDistance = Vector3.SqrMagnitude(huntEnemy.transform.position - stateMachine.Unit.transform.position);
            float sqrAttackRange = stateMachine.Unit.StatHandler.CurrentStat.AttackRange * stateMachine.Unit.StatHandler.CurrentStat.AttackRange;
            // 적이 사정거리 내에 존재한다면
            if (sqrDistance <= sqrAttackRange)
            {
                targetEnemy = huntEnemy;
            }
        }

        if (targetEnemy == null)
        {
            // loop 도중 외부에서 SpwanEnemyList의 내용물을 변경하는 경우 문제 발생하는 것을 대처하기 위해 while문으로 변경
            while (idx < enemyList.Count) 
            {
                // 예외 처리
                if (enemyList[idx] == null) 
                {
                    idx++;
                    continue;
                }

                float sqrDistance = Vector3.SqrMagnitude(enemyList[idx].transform.position - stateMachine.Unit.transform.position);
                float sqrAttackRange = stateMachine.Unit.StatHandler.CurrentStat.AttackRange * stateMachine.Unit.StatHandler.CurrentStat.AttackRange;
                // 적이 사정거리 내에 존재한다면
                if (sqrDistance <= sqrAttackRange)
                {
                    if(sqrDistance < minDistance)
                    {
                        minDistance = sqrDistance;
                        targetEnemy = enemyList[idx];
                    }
                }
                idx++;
            }
        }
        
        if(targetEnemy != null)
        {
            // TODO : 무적상태 확인 로직 필요
            stateMachine.Unit.SetTargetEnemy(targetEnemy);
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }
}
