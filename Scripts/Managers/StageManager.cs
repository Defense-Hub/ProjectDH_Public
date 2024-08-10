using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [field:Header("# Sheet Data")]
    [field:SerializeField ] public StageData StageData { get; private set; }
    [field: Header("# Stage Info")]
    [field: SerializeField] public int CurStageLevel { get; private set; }

    [SerializeField] private int curEnemySpawnNum;

    [field: Header("# Stage Enemy Info")]
    [field: SerializeField] public List<Enemy> SpawnEnemyList { get; private set; }

    [field: SerializeField] public Transform[] EnemyWayPoints { get; private set; }
    [SerializeField] private float spawnDelayTime;
    public event Action<float, Action> OnTimerEvent;
    public Timer timer { get; private set; }
    public bool IsBossStage { get; private set; }
    public bool IsStageClear { get; private set; }

    private WaitForSeconds wait;
    private Coroutine spawnEnemyCoroutine;
    private readonly int initHuntMissionStageLevel = 3;
    private readonly float mapChangeTime = 1f;
    
    [field: Header("# BossTest ")]
    [HideInInspector] public bool TestMode;
    [HideInInspector] public int TestWaveTime = 99999;
    [HideInInspector] public DEV_BossType DEV_BossType = DEV_BossType.WaterEnemy;
    [HideInInspector] public EnemyData DEV_BossData = new EnemyData(2, 10000, 10);


    private void Start()
    {
        GameManager.Instance.Stage = this;
        
        wait = new WaitForSeconds(spawnDelayTime);

        timer = gameObject.AddComponent<Timer>();
        OnTimerEvent += timer.StartTimer;
        
    }
    public void InitStageData()
    {
        if (TestMode) 
        {
            StageData.BossWaveTime = TestWaveTime;
            StageData.RecoveryTime = 0;
            StageData.MaxStageLevel = 1;
            StageData.BossStageLevel = 1;
        }
        // 방어코드
        CurStageLevel = 1;

        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyWayPoints[i] = transform.GetChild(i);
        }

        StageData = GameDataManager.Instance.StageData;
    }
    public void GameStart()
    {
        UIManager.Instance.UI_Interface.UpdateStageClearEnemyUI();
        OnTimerEvent?.Invoke( StageData.RecoveryTime, StartStage);
        SoundManager.Instance.PlayBgm(EBgm.Main);
    }
    
    private void StartStage()
    {
        IsStageClear = false;
        
        if (!IsBossStage && CurStageLevel != 0 && CurStageLevel %  StageData.BossStageLevel == 0)
        {
            IsBossStage = true;
            AnalyticsManager.Instance.AnalyticsStageCount();
            if (TestMode)
            {
                DEV_SpawnBossEnemy((EAddressableType)DEV_BossType);
            }
            else
            {
                SpawnBossEnemy();
            }
            OnTimerEvent?.Invoke( StageData.BossWaveTime, GameManager.Instance.System.GameOver);
        }
        else
        {
            if (spawnEnemyCoroutine != null)
            {
                StopCoroutine(spawnEnemyCoroutine);
            }

            spawnEnemyCoroutine = StartCoroutine(SpawnBasicEnemyCoroutine());
            OnTimerEvent?.Invoke( StageData.BasicWaveTime, StageClear);
        }
        UIManager.Instance.UI_Interface.UpdateStageStartEnemyUI();

        if (CurStageLevel == initHuntMissionStageLevel)
        {
            GameManager.Instance.System.SetHuntMissionUI();
        }
    }

    private IEnumerator SpawnBasicEnemyCoroutine()
    {
        //int enemySpawnNum = TestMode ? 1 : MaxEnemySpawnNum;

        while (curEnemySpawnNum < StageData. MaxEnemySpawnNum) // Enemy 소환 
        {
            Enemy enemy = null;
            enemy = GameManager.Instance.Pool.SpawnFromPool((int)EEnemyType.Basic).ReturnMyComponent<Enemy>();
            UIManager.Instance.UI_EnemyHpBar.SetHpBarToNormalEnemy(enemy);

            enemy.transform.position = EnemyWayPoints[0].position;
            SpawnEnemyList.Add(enemy);
            enemy.EnemyInit(CurStageLevel);

            curEnemySpawnNum++;
            yield return wait;
        }
    }

    private void SpawnBossEnemy()
    {
        Boss boss = GameManager.Instance.Pool.SpawnFromPool((int)EEnemyType.Boss).ReturnMyComponent<Boss>();
        boss.transform.position = EnemyWayPoints[0].transform.position;
        SpawnEnemyList.Add(boss);
        boss.EnemyInit(CurStageLevel);
    }

    private async void DEV_SpawnBossEnemy(EAddressableType bossType)
    {
        GameObject obj = await AddressableManager.Instance.InstantiateAsync(bossType, 1);
        Boss boss = obj.GetComponent<Boss>();
        boss.transform.position = EnemyWayPoints[0].transform.position;
        SpawnEnemyList.Add(boss);
        boss.DEV_EnemyInit(DEV_BossData);
    }

    private void StageInit() // 스테이지 초기화 함수
    {

        if (SpawnEnemyList.Count == 0)
        {
            PlayerDataManager.Instance.inGameData.ChangeGold( GameDataManager.Instance.GameCurrencyData.StageClearBonusGold);
        }
        
        curEnemySpawnNum = 0;
        
        // foreach 루프문은 컬렉션 반복중에 컬렉션 수정이 안된다.
        for (int i = SpawnEnemyList.Count - 1; i >= 0; i--)
        {
            Enemy enemy = SpawnEnemyList[i];

            enemy.HealthSystem.TakeDamage(int.MaxValue);
        }
    }

    private void StageClear()
    {
        if (CurStageLevel == StageData.MaxStageLevel)
        {
            GameManager.Instance.System.GameClear();
            return;
        }
        IsStageClear = true;
        
        StageInit(); 

        // 튜토리얼 스테이지 라면
        if (GameManager.Instance.Tutorial != null)
        {
            TutorialStage();
            return;
        }
        
        CurStageLevel++;
        
        // 테마 변경
        if (IsBossStage)
        {
            IsBossStage = false;
            Invoke(nameof(ChangeEnemy), mapChangeTime);
        }
        else
        {
            // 용암 이벤트
            if (CurStageLevel == StageData.BossStageLevel * 2)
            {
                GameManager.Instance.UnitSpawn.Controller.ActivateLava(StageData.RecoveryTime);
            }
        
            UIManager.Instance.UI_Interface.UpdateStageClearEnemyUI();
            OnTimerEvent?.Invoke(StageData.RecoveryTime, StartStage);
        }
        
        AnalyticsManager.Instance.AnalyticsStageEnd();
    }

    private async void ChangeEnemy()
    {
        UIManager.Instance.UI_Interface.UpdateStageClearEnemyUI();
        await ResourceManager.Instance.ChangeMapTheme();
        OnTimerEvent?.Invoke(StageData.RecoveryTime, StartStage);
    }

    public void DeActivateEnemy(Enemy enemy)
    {
        SpawnEnemyList.Remove(enemy);

        if(!IsStageClear)
            PlayerDataManager.Instance.inGameData.ChangeGold(GameDataManager.Instance.GameCurrencyData.EnemyPerKillGold);
        
        if ( CurStageLevel == StageData.BossStageLevel * 2 && SpawnEnemyList.Count == 0)
        {
            GameManager.Instance.UnitSpawn.Controller.DeActivateLava();
        }
    }

    public void CheckStageClear()
    {
        if(IsStageClear)
            return;
        
        if ((!IsBossStage && curEnemySpawnNum == StageData.MaxEnemySpawnNum && SpawnEnemyList.Count == 0) || (IsBossStage && SpawnEnemyList.Count == 0)) // 마지막 몬스터라면
        {
            StageClear();
        }
    }

    private void TutorialStage() // 튜토리얼 스테이지 이벤트
    {
        OnTimerEvent?.Invoke(0, null);  
        GameManager.Instance.Tutorial.TutorialUIController.DialogueEvent.StartDialouge();
    }
}
