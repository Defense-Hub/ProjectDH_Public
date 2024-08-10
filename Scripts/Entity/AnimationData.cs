using System;
using UnityEngine;

[Serializable]
public class AnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string moveParameterName = "Move";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string skill1ParameterName = "Skill1";
    [SerializeField] private string skill2ParameterName = "Skill2";
    [SerializeField] private string dieParameterName = "Die";
    [SerializeField] private string attackSpeedParameterName = "AttackSpeed";


    public int IdleParameterHash { get; private set; }
    public int MoveParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int Skill1ParameterHash { get; private set; }
    public int Skill2ParameterHash { get; private set; }
    public int DieParameterHash { get; private set; }
    public int AttackSpeedParameterHash { get; private set; }

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        MoveParameterHash = Animator.StringToHash(moveParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        Skill1ParameterHash = Animator.StringToHash(skill1ParameterName);
        Skill2ParameterHash = Animator.StringToHash(skill2ParameterName);
        DieParameterHash = Animator.StringToHash(dieParameterName);
        AttackSpeedParameterHash = Animator.StringToHash(attackSpeedParameterName);
    }
}