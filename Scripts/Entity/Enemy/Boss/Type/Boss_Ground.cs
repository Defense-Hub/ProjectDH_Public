using UnityEngine;

public class Boss_Ground : Boss
{
    public override void EnemyInit(int key)
    {
        base.EnemyInit(key);
        PatternHandler.OnStartPatternEvent += UseToughnessSkill;
    }

    public override void DEV_EnemyInit(EnemyData data)
    {
        base.DEV_EnemyInit(data);
        PatternHandler.OnStartPatternEvent += UseToughnessSkill;
    }

    protected override void EnemyDie()
    {
        base.EnemyDie();
        PatternHandler.OnStartPatternEvent -= UseToughnessSkill;
    }

    protected override void AddHealthEvent()
    {
        base.AddHealthEvent();
        HealthSystem.OnHealthChange += UIManager.Instance.UI_Interface.UpdateBossHPBarUI;
    }

    protected override void SubtractHealthEvent()
    {
        base.SubtractHealthEvent();
        HealthSystem.OnHealthChange -= UIManager.Instance.UI_Interface.UpdateBossHPBarUI;
    }

    private void UseToughnessSkill()
    {
        SkillHandler.CallSkill(EBossSkill.Toughness);
    }
}