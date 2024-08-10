using UnityEngine;

public class ActiveSkill : UnitSkill
{
    // TODO: 버튼한테 넘겨줘야하는 시전 함수 제작
    [SerializeField] private Sprite icon;

    public Sprite Icon => icon;

    public override void EndSkill()
    {
        // unit의 curPassiveSkill 비우기
        unit.SkillHandler.CurActiveSkill = null;
        targetEnemy = null;
        targetEnemies.Clear();
    }
}
