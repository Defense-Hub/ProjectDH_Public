using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameDataTable;
using EnemyDataTable;
using UnitDataTable;
using UGS;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameDataManager : Singleton<GameDataManager> // Json으로 로드하는 데이터
{
    public bool isRunTimeLoad;

    [field: Header("# Unit Data Info")]
    [field: SerializeField] public Dictionary<int, UnitBase> UnitBases { get; private set; } =
        new Dictionary<int, UnitBase>();

    [field: SerializeField] private Color commonUnitColor;
    [field: SerializeField] private Color rareUnitColor;
    [field: SerializeField] private Color uniqueUnitColor;
    [field: SerializeField] private Color legendaryUnitColor;
    [field: SerializeField] private Color epicUnitColor;

    public Color CommonUnitColor => commonUnitColor;
    public Color RareUnitColor => rareUnitColor;
    public Color UniqueUnitColor => uniqueUnitColor;
    public Color LegendaryUnitColor => legendaryUnitColor;
    public Color EpicUnitColor => epicUnitColor;

    [field: Header("#Unit Projectile Material")]
    [field: SerializeField] private Material fireProjectileMaterial;
    [field: SerializeField] private Material iceProjectileMaterial;
    [field: SerializeField] private Material groundProjectileMaterial;
    [field: SerializeField] private Material darkProjectileMaterial;
    [field: SerializeField] private Material waterProjectileMaterial;

    public Material FireProjectileMaterial => fireProjectileMaterial;
    public Material IceProjectileMaterial => iceProjectileMaterial;
    public Material GroundProjectileMaterial => groundProjectileMaterial;
    public Material DarkProjectileMaterial => darkProjectileMaterial;
    public Material WaterProjectileMaterial => waterProjectileMaterial;


    [field: Header("# Buff Data Info")]
    [field: SerializeField] public Dictionary<int, BuffData> BuffDatas { get; private set; } =
        new Dictionary<int, BuffData>();

    [field: Header("# Enemy Data Info")]
    [field: SerializeField] public Dictionary<int, EnemyData> EnemyDatas { get; private set; } =
        new Dictionary<int, EnemyData>();

    [field: Header("# Unit Spawn Probability Info")]
    [field: SerializeField] public List<UnitSpawnProbabilityData> SpawnProbability { get; private set; } =
        new List<UnitSpawnProbabilityData>();

    [field: Header("# Group Gamble Probability Info")]
    [field: SerializeField] public GroupGambleProbability GambleProbability { get; private set; } =
        new GroupGambleProbability();
    [field: SerializeField] public List<GroupGambleProbabilityData> GambleProbabilityDatas { get; private set; } =
        new List<GroupGambleProbabilityData>();
    
    [field: Header("# Combination Data Info")]
    [field: SerializeField] public List<CombinationData> CombinationDatas { get; private set; } =
        new List<CombinationData>();

    [field: Header("# Mission Data Info")]
    [field: SerializeField] public List<MissionData> MissionDatas { get; private set; } = new List<MissionData>();

    [field: Header("# Unit Min Max ID Data Info")]
    [field: SerializeField] public List<UnitMinMaxIDData> UnitMinMaxID { get; private set; } =
        new List<UnitMinMaxIDData>();

    [field: Header("# Unit Enforce Data Info")]
    [field: SerializeField] public List<UnitEnforceData> AdvancedUnitEnforceDatas { get; private set; } = new List<UnitEnforceData>();

    [field: SerializeField] public List<UnitEnforceData> BasicUnitEnforceDatas { get; private set; } = new List<UnitEnforceData>();

    [field: Header("# Hunt Enemy Data Info")]
    [field: SerializeField] public Dictionary<int, HuntEnemyData> HuntEnemyDatas { get; private set; } =
        new Dictionary<int, HuntEnemyData>();

    [field: Header("# Game Tip Data Info")]
    [field: SerializeField] public List<string> GameTipDatas { get; private set; } = new List<string>();
    
    [field: Header("# Game Currency Data Info")]
    [field: SerializeField] public GameCurrencyData GameCurrencyData { get; private set; } = new GameCurrencyData();
    
    [field: Header("# Stage Data Info")]
    [field: SerializeField] public StageData StageData { get; private set; } = new StageData();

    protected override void Awake()
    {
        base.Awake();
    }

    #region Data Load Code

    public async Task LoadGameData()
    {
        UnityGoogleSheet.LoadAllData();

        LoadLocalDatas();
        await LoadThumbnail();
    }
    

    private void LoadLocalDatas()
    {
        LoadLocalSpawnProbability();
        LoadLocalUnitData();
        LoadLocalEnemyData();
        LoadLocalGambleProbabilityData();
        LoadLocalCombinationData();
        LoadLocalBuffData();
        LoadLocalMissionData();
        LoadLocalEnforceData();
        LoadLocalHuntEnemyData();
        LoadLocalGameTipData();
        LoadLocalStageData();
        LoadLocalGameCurrencyData();
    }

    #region LoadSpawnProbability


    private void LoadLocalSpawnProbability()
    {
        foreach (var data in SpawnProbabilityDataSheet.GetList())
        {
            SetUnitSpawnProbability(data);
        }
    }

    private void SetUnitSpawnProbability(SpawnProbabilityDataSheet data)
    {
        UnitSpawnProbabilityData percentage = new UnitSpawnProbabilityData();

        percentage.percent[0] = data.commonPer;
        percentage.percent[1] = data.rarePer;
        percentage.percent[2] = data.uniquePer;
        percentage.percent[3] = data.legendryPer;
        SpawnProbability.Add(percentage);
    }

    #endregion

    #region LoadUnitData

    private void LoadLocalUnitData()
    {
        List<CommonUnitDataSheet> commonUnitDataList = CommonUnitDataSheet.CommonUnitDataSheetList;

        UnitMinMaxID.Add(new UnitMinMaxIDData(new int[2]
            { commonUnitDataList[0].ID, commonUnitDataList[commonUnitDataList.Count - 1].ID + 1 }));
        for (int i = 0; i < commonUnitDataList.Count; i++)
        {
            UnitBases.Add(commonUnitDataList[i].ID, new UnitBase(commonUnitDataList[i].ID, commonUnitDataList[i].Name,
                commonUnitDataList[i].BaseMoveSpeed, commonUnitDataList[i].UnitRank, commonUnitDataList[i].AttackType,
                commonUnitDataList[i].BaseAttackPower, commonUnitDataList[i].BaseAttackSpeed,
                commonUnitDataList[i].BaseAttackRange, commonUnitDataList[i].UnitDescription,
                commonUnitDataList[i].Splash,
                commonUnitDataList[i].Stun, commonUnitDataList[i].Slow));
        }

        List<RareUnitDataSheet> rareUnitDataList = RareUnitDataSheet.RareUnitDataSheetList;

        UnitMinMaxID.Add(new UnitMinMaxIDData(new int[2]
            { rareUnitDataList[0].ID, rareUnitDataList[rareUnitDataList.Count - 1].ID + 1 }));
        for (int i = 0; i < rareUnitDataList.Count; i++)
        {
            UnitBases.Add(rareUnitDataList[i].ID, new UnitBase(rareUnitDataList[i].ID, rareUnitDataList[i].Name,
                rareUnitDataList[i].BaseMoveSpeed, rareUnitDataList[i].UnitRank, rareUnitDataList[i].AttackType,
                rareUnitDataList[i].BaseAttackPower, rareUnitDataList[i].BaseAttackSpeed,
                rareUnitDataList[i].BaseAttackRange, rareUnitDataList[i].UnitDescription, rareUnitDataList[i].Splash,
                rareUnitDataList[i].Stun, rareUnitDataList[i].Slow));
        }

        List<UniqueUnitDataSheet> uniqueUnitDataList = UniqueUnitDataSheet.UniqueUnitDataSheetList;

        UnitMinMaxID.Add(new UnitMinMaxIDData(new int[2]
            { uniqueUnitDataList[0].ID, uniqueUnitDataList[uniqueUnitDataList.Count - 1].ID + 1 }));
        for (int i = 0; i < uniqueUnitDataList.Count; i++)
        {
            UnitBases.Add(uniqueUnitDataList[i].ID, new UnitBase(uniqueUnitDataList[i].ID, uniqueUnitDataList[i].Name,
                uniqueUnitDataList[i].BaseMoveSpeed, uniqueUnitDataList[i].UnitRank, uniqueUnitDataList[i].AttackType,
                uniqueUnitDataList[i].BaseAttackPower, uniqueUnitDataList[i].BaseAttackSpeed,
                uniqueUnitDataList[i].BaseAttackRange, uniqueUnitDataList[i].UnitDescription,
                uniqueUnitDataList[i].Splash,
                uniqueUnitDataList[i].Stun, uniqueUnitDataList[i].Slow));
        }

        List<LegendryUnitDataSheet> legendryUnitDataList = LegendryUnitDataSheet.LegendryUnitDataSheetList;

        UnitMinMaxID.Add(new UnitMinMaxIDData(new int[2]
            { legendryUnitDataList[0].ID, legendryUnitDataList[legendryUnitDataList.Count - 1].ID + 1 }));
        for (int i = 0; i < legendryUnitDataList.Count; i++)
        {
            UnitBases.Add(legendryUnitDataList[i].ID, new UnitBase(legendryUnitDataList[i].ID,
                legendryUnitDataList[i].Name, legendryUnitDataList[i].BaseMoveSpeed, legendryUnitDataList[i].UnitRank,
                legendryUnitDataList[i].AttackType,
                legendryUnitDataList[i].BaseAttackPower, legendryUnitDataList[i].BaseAttackSpeed,
                legendryUnitDataList[i].BaseAttackRange, legendryUnitDataList[i].UnitDescription,
                legendryUnitDataList[i].Splash,
                legendryUnitDataList[i].Stun, legendryUnitDataList[i].Slow));
        }

        List<EpicUnitDataSheet> epicUnitDataList = EpicUnitDataSheet.EpicUnitDataSheetList;

        //UnitMinMaxID.Add(new UnitMinMaxIDData(new int[2] { epicUnitDataList[0].ID, epicUnitDataList[epicUnitDataList.Count - 1].ID + 1 }));
        for (int i = 0; i < epicUnitDataList.Count; i++)
        {
            UnitBases.Add(epicUnitDataList[i].ID, new UnitBase(epicUnitDataList[i].ID, epicUnitDataList[i].Name,
                epicUnitDataList[i].BaseMoveSpeed, epicUnitDataList[i].UnitRank, epicUnitDataList[i].AttackType,
                epicUnitDataList[i].BaseAttackPower, epicUnitDataList[i].BaseAttackSpeed,
                epicUnitDataList[i].BaseAttackRange, epicUnitDataList[i].UnitDescription, epicUnitDataList[i].Splash,
                epicUnitDataList[i].Stun, epicUnitDataList[i].Slow));
        }

        List<TranscendentUnitDataSheet> transcendentUnitDataList =
            TranscendentUnitDataSheet.TranscendentUnitDataSheetList;

        //UnitMinMaxID.Add(new UnitMinMaxIDData(new int[2] { transcendentUnitDataList[0].ID, transcendentUnitDataList[transcendentUnitDataList.Count - 1].ID + 1 }));
        for (int i = 0; i < transcendentUnitDataList.Count; i++)
        {
            UnitBases.Add(transcendentUnitDataList[i].ID, new UnitBase(transcendentUnitDataList[i].ID,
                transcendentUnitDataList[i].Name, transcendentUnitDataList[i].BaseMoveSpeed,
                transcendentUnitDataList[i].UnitRank, transcendentUnitDataList[i].AttackType,
                transcendentUnitDataList[i].BaseAttackPower, transcendentUnitDataList[i].BaseAttackSpeed,
                transcendentUnitDataList[i].BaseAttackRange, transcendentUnitDataList[i].UnitDescription,
                transcendentUnitDataList[i].Splash,
                transcendentUnitDataList[i].Stun, transcendentUnitDataList[i].Slow));
        }
    }

    #endregion

    #region LoadEnemyData

    private void LoadLocalEnemyData()
    {
        foreach (var data in BasicEnemyDataSheet.GetList())
        {
            EnemyDatas.Add(data.Key, new EnemyData(data.MoveSpeed, data.MaxHealth, data.Toughness));
        }

        foreach (var data in BossEnemyDataSheet.GetList())
        {
            EnemyDatas.Add(data.Key, new EnemyData(data.MoveSpeed, data.MaxHealth, data.Toughness));
        }
    }

    #endregion

    #region LoadGamble
    
    private void LoadLocalGambleProbabilityData()
    {
        foreach (var data in GroupGambleProbabilityDataSheet.GetList())
        {
            SetGambleData(data);
        }
    }

    private void SetGambleData(GroupGambleProbabilityDataSheet data)
    {
        GambleProbabilityDatas.Add(new GroupGambleProbabilityData()
        {
            GambleProbability = data.GambleProbability,
            RequiredGemStone =  data.RequriedGemStone
        });
    }
    
    #endregion

    #region LoadCombinationData
    
    private void LoadLocalCombinationData()
    {
        foreach (var data in CombinationDataSheet.GetList())
        {
            SetCombinationData(data);
        }
    }

    private void SetCombinationData(CombinationDataSheet data)
    {
        CombinationDatas.Add(new CombinationData()
        {
            targetID = data.targetID,
            materialIDs = data.materialID
        });
    }

    #endregion

    #region LoadBuffData

    private void LoadLocalBuffData()
    {
        foreach (var data in BuffDataSheet.GetList())
        {
            SetBuffData(data);
        }
    }

    private void SetBuffData(BuffDataSheet data)
    {
        BuffDatas.Add(data.ID, new BuffData(data.ID,
            data.StatChangeType, data.MoveSpeedDelta, data.AttackPowerDelta, data.AttackSpeedDelta,
            data.AttackRangeDelta));
    }

    #endregion

    #region LoadMissionData

    private void LoadLocalMissionData()
    {
        foreach (var data in UnitMissionDataSheet.GetList())
        {
            SetBuffData(data);
        }
    }

    private void SetBuffData(UnitMissionDataSheet data)
    {
        MissionDatas.Add(new MissionData(data.MissionTitle, data.MissionDesc, data.index, data.MatirialID,
            data.GoldReward, data.GemStoneReward));
    }

    #endregion

    #region Load Unit Enforce Data

    private void LoadLocalEnforceData()
    {
        LoadLocalAdvancedUnitEnforceData();
        LoadLocalBasicUnitEnforceData();
    }

    private void LoadLocalAdvancedUnitEnforceData()
    {
        foreach (var data in AdvancedUnitEnforceDataSheet.GetList())
        {
            AdvancedUnitEnforceDatas.Add(new UnitEnforceData()
            {
                EnforceValue = (data.EnforceValue / 100f) + 1f,
                RequiredCurrency = data.RequiredGemStone
            });
        }
    }

    private void LoadLocalBasicUnitEnforceData()
    {
        foreach (var data in BasicUnitEnforceDataSheet.GetList())
        {
            BasicUnitEnforceDatas.Add(new UnitEnforceData()
            {
                EnforceValue = (data.EnforceValue / 100f) + 1f,
                RequiredCurrency = data.RequiredGold
            });
        }
    }

    #endregion

    #region Load Hunt Enemy Data

    private void LoadLocalHuntEnemyData()
    {
        foreach (var data in HuntEnemyDataSheet.GetList())
        {
            HuntEnemyDatas.Add(data.Level, new HuntEnemyData()
            {
                MaxHealth = data.MaxHealth,
                MoveSpeed = data.MoveSpeed,
                Toughness = data.Toughness,
                GemStoneReward = data.GemStoneReward
            });
        }
    }

    #endregion

    #region Load Thumbnail

    private async Task LoadThumbnail()
    {
        int idx = 0;
        SpriteAtlas atlas =  await AddressableManager.Instance.LoadAsset<SpriteAtlas>(EAddressableType.Thumbnail, idx);

        foreach (var enumValue in Enum.GetValues(typeof(EUnitRCode)))
        {
            int intEnumValue = (int)enumValue;
            Sprite loadThumbnail = atlas.GetSprite(enumValue.ToString());
        
            if (UnitBases.TryGetValue(intEnumValue, out var data))
            {
                data.Thumbnail = loadThumbnail;
            }
            else
            {
                Debug.LogError($"{intEnumValue} => Thumbnail Load Error");
            }
        }
        
        // foreach (var enumValue in Enum.GetValues(typeof(EUnitRCode)))
        // {
        //     int intEnumValue = (int)enumValue;
        //     Texture2D loadThumbnail = await AddressableManager.Instance.LoadAsset<Texture2D>(EAddressableType.Thumbnail, idx++);
        //
        //     if (UnitBases.TryGetValue(intEnumValue, out var data))
        //     {
        //         // 텍스처 2D의 전체 영역 설정
        //         Rect rect = new Rect(0, 0, loadThumbnail.width, loadThumbnail.height);
        //         // Sprite 생성 (텍스쳐, 영역, 피봇, PixelPerUnit = 100)
        //         Sprite sprite = Sprite.Create(loadThumbnail, rect, new Vector2(0.5f, 0.5f));
        //         data.Thumbnail = sprite;
        //     }
        //     else
        //     {
        //         Debug.LogError($"{intEnumValue} => Thumbnail Load Error");
        //     }
        // }
    }

    #endregion

    #region Load Game Tips

    private void LoadLocalGameTipData()
    {
        foreach (var data in GameTipDataSheet.GetList())
        {
            GameTipDatas.Add(data.GameTip);
        }
    }

    #endregion
    
    #region Load Stage Data

    private void LoadLocalStageData()
    {
        foreach (var data in StageDataSheet.GetList())
        {
            StageData = new StageData()
            {
                MaxStageLevel = data.MaxStageLevel,
                BossStageLevel = data.bossStageLevel,
                MaxEnemySpawnNum = data.MaxEnemySpawnNum,
                BasicWaveTime = data.BasicWaveTime,
                BossWaveTime = data.BossWaveTime,
                RecoveryTime = data.RecoveryTime
            };
        }
    }

    #endregion
   
    #region Load Game Currency Data
    private void LoadLocalGameCurrencyData()
    {
        foreach (var data in GameCurrencyDataSheet.GetList())
        {
            GameCurrencyData = new GameCurrencyData()
            {
                InitSummonGold = data.InitSummonGold,
                SpawnProbabilityEnforceGold = data.SpawnProbabilityEnforceGold,
                StageClearBonusGold = data.StageClearGold,
                EnemyPerKillGold = data.EnemyPerKillGold,
                InitPlayerGold = data.InitPlayerGold,
                InitPlayerGemStone = data.InitPlayerGemStone
            };
        }
    }

    #endregion
    #endregion
}