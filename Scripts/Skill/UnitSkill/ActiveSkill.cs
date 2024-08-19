using UnityEngine;

public class ActiveSkill : UnitSkill
{
    // TODO: 버튼한테 넘겨줘야하는 시전 함수 제작
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;

    public override void EndSkill()
    {
        // unit의 curPassiveSkill 비우기
        GameManager.Instance.PlayerSkill.DeactivateSkill();
        unit.SkillHandler.CurActiveSkill = null;
        targetEnemy = null;
        targetEnemies.Clear();
    }

    // Active 상태일 때 false, 타겟이 없을 때 false
    public bool CanActiveSkill()
    {
        return isTargetOn;
    }
}
