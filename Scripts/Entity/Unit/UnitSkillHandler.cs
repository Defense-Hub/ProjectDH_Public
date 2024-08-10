using System.Collections.Generic;
using UnityEngine;

public class UnitSkillHandler : MonoBehaviour
{
    // Skill 시전 할지 말지만 결정
    // 시전 시 행동은 각 Skill들이 사용
    [SerializeField] private List<PassiveSkill> passiveSkillList;
    [SerializeField] private List<ActiveSkill> activeSkillList;
    public Unit Unit;
    public PassiveSkill CurPassiveSkill { get; set; }
    public ActiveSkill CurActiveSkill { get; set; }

    public void Init(Unit unit)
    {
        Unit = unit;
    }

    public void InitActiveSkill()
    {
        foreach (ActiveSkill activeSkill in activeSkillList)
        {
            activeSkill.Init(Unit);
            GameManager.Instance.PlayerSkill.AddSkill(activeSkill);
        }
        CurActiveSkill = activeSkillList[0];
    }

    public bool HasActivableSkill()
    {
        // 여러개의 스킬이 동시에 나갈 수도 있고, 하나만 나갈 수도 있다.
        foreach (PassiveSkill passiveSkill in passiveSkillList)
        {
            if (RandomEvent.GetBoolRandomResult(passiveSkill.Probability))
            {
                CurPassiveSkill = passiveSkill;
                passiveSkill.Init(Unit);
                passiveSkill.UseSkill();
                return true;
            }
        }
        return false;
    }

    public Sprite GetActiveSkillIcon()
    {
        return activeSkillList[0].Icon;
    }
}
