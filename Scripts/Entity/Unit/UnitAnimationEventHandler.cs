using System;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnCheck;
    public event Action OnSkillAttack;
    public event Action OnSkillEnd;
    public event Action OnSkillCount;
    
    public void CallAttack()
    {
        OnAttack?.Invoke();
    }

    public void CallCheckTarget()
    {
        OnCheck?.Invoke();
    }

    public void CallSkillAttack()
    {
        OnSkillAttack?.Invoke();    
    }

    public void CallSkillCount()
    {
        OnSkillCount?.Invoke();
    }

    public void CallSkillEnd()
    {
        OnSkillEnd?.Invoke();
    }
}