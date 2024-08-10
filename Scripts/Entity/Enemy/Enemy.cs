using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
    [field: SerializeField] public EnemyStat Stat { get; set; }
    [field: SerializeField] public Transform[] WayPoints { get; private set; }
    public EnemyData EnemyData { get; set; }
    public HealthSystem HealthSystem { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }
    public int TargetIndex { get; private set; }

    protected WaitForSeconds dieAnimLength;
    protected Coroutine dieCoroutine;
    
    protected override void Awake()
    {
        base.Awake();
        HealthSystem = GetComponent<HealthSystem>();
        StateMachine = new EnemyStateMachine(this);
        SetEnemyWayPoint();
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void CallDamageTXT(float damage, Transform transform)
    {
        UIManager.Instance.DamageUI(damage, transform);
    }

    // Enemy 초기화 함수
    public virtual void EnemyInit(int key)
    {
        EnemyData = GameDataManager.Instance.EnemyDatas[key].GetEnemyData();

        SetEnemyState();
    }

    protected void SetEnemyState()
    {
        Stat = new EnemyStat(EnemyData.MoveSpeed, EnemyData.MaxHealth, EnemyData.Toughness);
        
        StateMachine.ChangeState(StateMachine.MoveState);
        HealthSystem.InitHealthSystem(Stat.MaxHealth);
        HealthSystem.SetWayPoint(EffectWayPoint);
        SetTargetIndex(1);
        AddHealthEvent();
    }

    public virtual void DEV_EnemyInit(EnemyData data)
    {
        EnemyData = data;

        SetEnemyState();
    }

    protected virtual void AddHealthEvent()
    {
        HealthSystem.OnHit += CallDamageTXT;
        HealthSystem.OnDie += EnemyDie;
        HealthSystem.OnDie += UIManager.Instance.UI_Interface.UpdateEnemyCountUI;
    }

    protected virtual void SubtractHealthEvent()
    {
        HealthSystem.OnHit -= CallDamageTXT;
        HealthSystem.OnDie -= EnemyDie;
        HealthSystem.OnDie -= UIManager.Instance.UI_Interface.UpdateEnemyCountUI;
    }

    public void SetEnemyWayPoint() 
    {
        if (WayPoints.Length == 0)
            WayPoints = GameManager.Instance.Stage.EnemyWayPoints;
    }

    public void SetTargetIndex(int index)
    {
        TargetIndex = index;
    }

    protected virtual void EnemyDie()
    {
        GameManager.Instance.Stage.DeActivateEnemy(this);

        if (dieCoroutine != null)
        {
            StopCoroutine(dieCoroutine);
        }
        dieCoroutine = StartCoroutine(WaitForDieAnim());
    }

    IEnumerator WaitForDieAnim()
    {
        Animator.SetTrigger(AnimationData.DieParameterHash);
        if (dieAnimLength == null)
        {
            dieAnimLength = new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        yield return dieAnimLength;

        GameManager.Instance.Stage.CheckStageClear();
        gameObject.SetActive(false);
        SubtractHealthEvent();
    }
}
