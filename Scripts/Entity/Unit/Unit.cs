using UnityEngine;

public class Unit : Entity //IPointerDownHandler
{
    public UnitStateMachine StateMachine { get; private set; }
    public UnitStatHandler StatHandler { get; private set; }
    public UnitDataHandler DataHandler { get; private set; }
    public UnitSkillHandler SkillHandler { get; private set; }
    public UnitAnimationEventHandler AnimationEventHandler { get; private set; }
    [SerializeField] private SpriteRenderer UnitTierIndicator;

    // SO로 받아오는 기본 스텟
    [Header("# Stat정보")]
    [SerializeField] private int id;
    [SerializeField] private UnitBaseSO baseStat;
    [SerializeField] private float searchDelay;

    private Enemy targetEnemy;

    public int Id => id;
    public Enemy TargetEnemy => targetEnemy;
    public float SearchDelay => searchDelay;

    [Header("# buff Test용 변수")]
    public UnitData D_UnitData;
    public UnitStat D_BaseStat;
    public UnitStat D_CurStat;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        StatHandler = new UnitStatHandler();
        DataHandler = new UnitDataHandler();
        
        // 로드가 잘 된경우
        if (GameDataManager.Instance.UnitBases.ContainsKey(id))
        {
            Debug.Log("로드 성공"); 
            StatHandler.Init(GameDataManager.Instance.UnitBases[id]);
            DataHandler.InitData(GameDataManager.Instance.UnitBases[id]);
        }
        else
        {
            Debug.Log("로드 실패");
            StatHandler.Init(baseStat);
            DataHandler.InitData(baseStat);
        }

        SetTierIndicator();
        if (TryGetComponent(out UnitSkillHandler SkillHandler))
        {
            this.SkillHandler = SkillHandler;
            SkillHandler.Init(this);
            SkillHandler.InitActiveSkill();
        }

        AnimationEventHandler = GetComponentInChildren<UnitAnimationEventHandler>();

        StatHandler.UpdateCharacterStat();
        GameManager.Instance.FieldManager.OnBuffChange += StatHandler.UpdateCharacterStat;

        ///////////////////////////////////////////
        ///DEV ONLY
        D_UnitData = DataHandler.Data;
        D_BaseStat = StatHandler.BaseStat;
        D_CurStat = StatHandler.CurrentStat;
        ///DEV ONLY
        ///////////////////////////////////////////

        if (StateMachine == null)
        {
            StateMachine = new UnitStateMachine(this);   
        }
        StateMachine.ChangeState(StateMachine.IdleState);
    }

    private void SetTierIndicator()
    {
        EUnitRank curUnitRank = DataHandler.Data.UnitRank;
        switch (curUnitRank)
        {
            case EUnitRank.Common:
                UnitTierIndicator.color = GameDataManager.Instance.CommonUnitColor;
                break;

            case EUnitRank.Rare:
                UnitTierIndicator.color = GameDataManager.Instance.RareUnitColor;
                break;

            case EUnitRank.Unique:
                UnitTierIndicator.color = GameDataManager.Instance.UniqueUnitColor;
                break;

            case EUnitRank.Legendary:
                UnitTierIndicator.color = GameDataManager.Instance.LegendaryUnitColor;
                break;
        }
    }

    public void DisableUnit()
    {
        GameManager.Instance.FieldManager.OnBuffChange -= StatHandler.UpdateCharacterStat;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void InitTarget()
    {
        targetEnemy = null;
    }

    public void SetTargetEnemy(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    // For Dev Only
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(transform.position, StatHandler.CurrentStat.AttackRange);
    }
}