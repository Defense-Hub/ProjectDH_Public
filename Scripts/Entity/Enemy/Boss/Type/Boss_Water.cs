using System.Collections;
using UnityEngine;

public class Boss_Water : Boss
{
    [Header("# 분열 스킬 발동 체력 조건")]
    [SerializeField] private float splitPercentage;
    [field : Header("# 분열 스킬 최대 사용 가능 횟수")]
    [field : SerializeField] public int MaxSplitCount {  get; private set; }
    [field:Header("# 분열 스킬 현재 사용 가능 횟수")]
    [field: SerializeField] public int CurSplitCount { get; private set; }

    [SerializeField] private GameObject countDownUIPrefab;
    public UI_CountDown ui_CountDown;
    private Vector3 originScale;

    protected override void Awake()
    {
        base.Awake();
        originScale = transform.localScale;
    }

    public override void EnemyInit(int key)
    {
        base.EnemyInit(key);
        PatternHandler.OnHealthChangePatternEvent += UseSplitSkill;
        CurSplitCount = MaxSplitCount;
        InitUI();
        transform.localScale = originScale;
        UIManager.Instance.UI_Interface.ActivateWBWarningTXT();
    }

    public override void DEV_EnemyInit(EnemyData data)
    {
        base.DEV_EnemyInit(data);
        PatternHandler.OnHealthChangePatternEvent += UseSplitSkill;
        CurSplitCount = MaxSplitCount;
        InitUI();
        transform.localScale = originScale;
        UIManager.Instance.UI_Interface.ActivateWBWarningTXT();
    }

    protected override void EnemyDie()
    {
        base.EnemyDie();
        PatternHandler.OnHealthChangePatternEvent -= UseSplitSkill;
        // 마지막 한 마리가 죽을 때 == 스테이지 끝날 때 DeActivate
        if(GameManager.Instance.Stage.SpawnEnemyList.Count == 0 && CurSplitCount == 0)
            UIManager.Instance.UI_Interface.DeActivateWBWarningTXT();
            if (ui_CountDown != null)
                ui_CountDown.DestroyUI();
    }

    public void SplitBossInit(EnemyData enemyData, EnemyStat curData, int splitCount, UI_CountDown ui)
    {
        if(ui_CountDown!=null) 
            ui_CountDown = ui;

        EnemyData = enemyData;

        Stat = new EnemyStat(GetRandomSpeed(enemyData.MoveSpeed), curData.MaxHealth, enemyData.Toughness);

        // 목표 지점 바라보기
        transform.rotation = Quaternion.LookRotation((WayPoints[TargetIndex].transform.position - transform.position).normalized);
        
        PatternHandler.OnHealthChangePatternEvent += UseSplitSkill;
        HealthSystem.InitHealthSystem(HealthSystem.GetCurHealth());
        HealthSystem.SetWayPoint(EffectWayPoint);
        AddHealthEvent();

        transform.localScale *= 0.7f;
        CurSplitCount = splitCount;
    }

    private void InitUI()
    {
        if (MaxSplitCount == CurSplitCount)
        {
            GameObject prefab = Instantiate(countDownUIPrefab);
            ui_CountDown = prefab.GetComponent<UI_CountDown>();
            ui_CountDown.InitUI();
        }
    }

    public void Split()
    {
        CurSplitCount--;
    }

    public void UseSplitSkill(float healthPercentage)
    {
        if (healthPercentage <= splitPercentage && HealthSystem.GetCurHealth()!=0 && CurSplitCount > 0)
        {            
            SkillHandler.CallSkill(EBossSkill.SplitSkill);
        }
    }

    private float GetRandomSpeed(float baseSpeed)
    {
        return Random.Range(baseSpeed, baseSpeed + 1f);
    }
}