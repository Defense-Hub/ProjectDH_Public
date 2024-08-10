using System;

[Serializable]
public class BossSkillInfo
{
    public EBossSkill EBossSkill;
    public ESkillType ESkillType;
    public BossSkill Skill;
    public event Action OnSkill;

    public void CallSkill()
    {
        OnSkill?.Invoke();
    }
}
