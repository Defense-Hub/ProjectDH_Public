
public class Boss : Enemy
{
    public BossSkillHandler SkillHandler;
    public BossPatternHandler PatternHandler;
    
    protected override void Awake()
    {
        base.Awake();
        SkillHandler = GetComponent<BossSkillHandler>();
        PatternHandler = GetComponent<BossPatternHandler>();

        SkillHandler.Init(this);
        PatternHandler.Init(this);
    }

    public override void EnemyInit(int key)
    {
        base.EnemyInit(key);
        UIManager.Instance.UI_Interface.UpdateBossHPBarUI(1);
        SkillHandler.FinishSkillCast();
        PatternHandler.BossPattern();
    }

    public override void DEV_EnemyInit(EnemyData data)
    {
        base.DEV_EnemyInit(data);
        UIManager.Instance.UI_Interface.UpdateBossHPBarUI(1);
        SkillHandler.FinishSkillCast();
        PatternHandler.BossPattern();
    }
    protected override void EnemyDie()
    {
        SkillHandler.EndSkill();
        base.EnemyDie();
    }

    protected override void AddHealthEvent()
    {
        base.AddHealthEvent();
        HealthSystem.OnHealthChange += PatternHandler.CheckHealth;
        HealthSystem.OnDie += PatternHandler.CallDiePatternEvent;
    }

    protected override void SubtractHealthEvent()
    {
        base.SubtractHealthEvent();
        HealthSystem.OnHealthChange -= PatternHandler.CheckHealth;
        HealthSystem.OnDie -= PatternHandler.CallDiePatternEvent;
    }
}
