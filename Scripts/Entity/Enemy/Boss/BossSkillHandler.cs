using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossSkillHandler : MonoBehaviour
{
    [HideInInspector] public WaitForSeconds skillTermValue;
    [HideInInspector] public WaitForSeconds firSkillDelay;

    [Header("# 스킬 리스트")]
    public BossSkillInfo[] Skills;
    [Header("# 스킬 딕셔너리")]
    public Dictionary<int, BossSkillInfo> SkillDictionary;
    [field: Header("# 스킬 시전 중인지 여부")]
    [field: SerializeField] public bool IsCasting { get; private set; }
    [Header("# 스킬 사이 텀")]
    [SerializeField][Range(0.5f, 15f)] private float skillTerm;
    [Header("# 첫 스킬 딜레이")]
    [SerializeField] private float firstSkillDelay;
    [Header("# 쿨타임 스킬 리스트")]
    [SerializeField] private List<EBossSkill> coolTimeSkills;


    private SkillInfo skillInfo = new SkillInfo();
    public SkillInfo SkillInfo => skillInfo;

    private Boss boss;

    private void Start()
    {
        InitializeSkillList();
        skillTermValue = new WaitForSeconds(skillTerm);
        firSkillDelay = new WaitForSeconds(firstSkillDelay);
    }

    private void InitializeSkillList()
    {
        SkillDictionary = new Dictionary<int, BossSkillInfo>();

        // 클로저 문제 - 람다식이 루프가 종료된 이후에 실행되는 경우 발생 => 로컬 변수 선언하여 해결
        for (int i = 0; i<Skills.Length; i++)
        {
            BossSkillInfo skill = Skills[i];
            Skills[i].Skill.Init(boss);
            SkillDictionary[(int)skill.EBossSkill] = skill;
            skill.OnSkill += () => skill.Skill.UseSkill();

            if (skill.ESkillType == ESkillType.CoolTime)
            {
                coolTimeSkills.Add(skill.EBossSkill);
            }
        }
    }

    public void Init(Boss boss)
    {
        this.boss = boss;
    }

    public void CallSkill(EBossSkill eBossSkill)
    {
        Debug.Log($"{eBossSkill} 스킬 사용");
        IsCasting = true;
        SkillDictionary[(int)eBossSkill].CallSkill(); 
    }

    public void FinishSkillCast()
    {
        IsCasting=false;
        skillInfo = default;
    }

    public void EndSkill()
    {
        foreach (var key in SkillDictionary.Keys)
        {
             SkillDictionary[key].Skill.EndSkill();
        }
    }

    // TODO :: 랜덤으로 스킬 사용하는 함수 추가
    // 쿨타임 스킬들 우선순위에 따라 실행하게 하는 함수
    public void UseCoolTimeSkill()
    {
        // coolTimeSkills의 인덱스가 스킬 우선순위
        foreach (var bossSkill in coolTimeSkills)
        {
            CoolTimeSkill curSkill = SkillDictionary[(int)bossSkill].Skill as CoolTimeSkill;
            // 쿨타임 다 돈 스킬 존재하면
            if (curSkill.IsReadyToUse() && !IsCasting)
            {
                // 한 번에 하나의 스킬만 실행
                CallSkill(bossSkill);
                break;
            }
        }
    }

    public bool IsEmptyCoolTimeSkillList()
    {
        if (coolTimeSkills != null) return false;
        else return true;
    }

    public void SetSkillInfo(ESkillType eSkillType, float animSpeedMultiplier, int animParameterHash, Action action = null)
    {
        skillInfo.skillType = eSkillType;
        skillInfo.animSpeedMultiplier = animSpeedMultiplier;
        skillInfo.animParameterHash = animParameterHash;
        skillInfo.onSkill += action;
    }

    public void CheckSkillEnd()
    {
        StartCoroutine(StartCheckSkillEnd());
    }

    private IEnumerator StartCheckSkillEnd()
    {
        int animParaHash = SkillInfo.animParameterHash;
        boss.Animator.SetFloat(boss.AnimationData.AttackSpeedParameterHash, SkillInfo.animSpeedMultiplier);
        boss.Animator.SetBool(animParaHash, true);
        
        float curAnimationLength = boss.Animator.GetCurrentAnimatorClipInfo(0).Length / SkillInfo.animSpeedMultiplier;
        float startTime = Time.time;
        float nowTime = 0f;

        while(nowTime < curAnimationLength)
        {
            nowTime = Time.time - startTime;
            yield return null;
        }
        boss.Animator.SetBool(animParaHash, false);
    }
}