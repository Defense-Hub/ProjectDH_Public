using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusHandler : MonoBehaviour
{
    private List<IStatus> statusList = new List<IStatus>();
    public List<IStatus> StatusList => statusList;
    // CC 걸린 상태인 것을 알 수 있게 체크.
    public bool IsHardCC { get; set; }
    
    private Slow slow = new Slow();
    private Stun stun = new Stun();
    public Coroutine coroutine;

    public void ApplyStatusEffect(ESpecialAttackType specialAttackType, SpecialAttack specialAttackData, Enemy target)
    {
        AddStatusEffect(specialAttackType);

        for (int i = statusList.Count - 1; i >= 0; i--) 
        {
            statusList[i].Apply(this, specialAttackData, target);
            statusList.RemoveAt(i);
        }
    }

    private void AddStatusEffect(ESpecialAttackType specialAttackType)
    {
        // 두마리가 동시에 slow를 걸어요
        // 코루틴을 쓰면, 걍 stopCoroutine 걸고 다시 돌리면 됨.
        foreach (ESpecialAttackType flagToCheck in Enum.GetValues(typeof(ESpecialAttackType)))
        {
            if (specialAttackType.HasFlag(flagToCheck))
            {
                switch (flagToCheck)
                {
                    case ESpecialAttackType.Slow:
                        AddStatus(slow);
                        break;
                    case ESpecialAttackType.Stun:
                        AddStatus(stun);
                        break;
                }
            }
        }
    }

    // monobehavior를 가지고 있지 않은 하위 객체들의 Coroutine을 돌려주기 위한 함수
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void StopRunningCoroutine()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }


    public void AddStatus(IStatus status)
    {
        statusList.Add(status);
    }
}