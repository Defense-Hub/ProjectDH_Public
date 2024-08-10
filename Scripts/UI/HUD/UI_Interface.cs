using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

using Sequence = DG.Tweening.Sequence;

public class UI_Interface : UI_HUD
{
    [Header("Summon Gold")]
    [SerializeField] private int summonGold = 30;

    [Header("CountDown UI Transform")]
    public Transform countDownTransform;

    private UnitSpawnManager _unitSpawnManager;
    private Slider bossHPBarSlider;
    private int curRemainEnemyNum;

    private void Start()
    {
        UIManager.Instance.UI_Interface = this;
        PlayerDataManager.Instance.OnUISetting += SetUI;
    }
    
    private void SetUI()
    {
        Init();
        GameManager.Instance.Stage.timer.OnChangeTimerText += UpdateStageTimer;
        GameManager.Instance.Stage.timer.OnEndEvent += UpdateStageTXT;
        PlayerDataManager.Instance.inGameData.OnChangeGold += UpdateGoldText;
        PlayerDataManager.Instance.inGameData.OnChangeGemStone += UpdateGemStoneText;
        PlayerDataManager.Instance.inGameData.OnChangeGold += CheckSummonBtn;
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(Objects));

        _unitSpawnManager = GameManager.Instance.UnitSpawn;

        GetText((int)Texts.TXT_Gold).text = PlayerDataManager.Instance.inGameData.Gold.ToString();
        GetText((int)Texts.TXT_GemStone).text = PlayerDataManager.Instance.inGameData.GemStone.ToString();
        GetText((int)Texts.TXT_SummonGold).text = PlayerDataManager.Instance.inGameData.UnitSpawnRequriedGold.ToString();
        
        GetButton((int)Buttons.Btn_Settings).AddOnClickEvent(SettingUIOnClickEvent);
        GetButton((int)Buttons.Btn_Reinforce).AddOnClickEvent(ReinforceOnClickEvent);
        GetButton((int)Buttons.Btn_Summon).AddOnClickEvent(SummonOnClickEvent);
        GetButton((int)Buttons.Btn_Myth).AddOnClickEvent(MythOnClickEvent);
        GetButton((int)Buttons.Btn_Gacha).AddOnClickEvent(GachaOnClickEvent);
        GetButton((int)Buttons.Btn_Misson).AddOnClickEvent(MissionOnClickEvent);
        
        GetObject((int)Objects.Object_BossUI).SetActive(false);
        GetObject((int)Objects.Object_MonsterUI).SetActive(false);
    }

    private void UpdateStageTimer(int currentTimer)
    {
        // 타이머 디스플레이 업데이트
        TimeSpan timerSpan = TimeSpan.FromSeconds(currentTimer);
        // Debug.Log("Timer Update");
        GetText((int)Texts.TXT_Timer).text = string.Format("{0:D2}:{1:D2}", timerSpan.Minutes, timerSpan.Seconds);
    }

    private void UpdateGoldText(int amount)   
    {           
        GetText((int)Texts.TXT_Gold).text = amount.ToString();
    }

    private void UpdateGemStoneText(int amount)
    {
        GetText((int)Texts.TXT_GemStone).text = amount.ToString();
    }

    #region 몬스터 UI
    // TODO :: 보스 UI는 동적생성 사용
    // 스테이지 몬스터 종류에 따라 몬스터 UI 업데이트
    public void UpdateStageStartEnemyUI()
    {
        if (GameManager.Instance.Stage.IsBossStage)
        {
            GetObject((int)Objects.Object_MonsterUI).SetActive(false);
            GetObject((int)Objects.Object_BossUI).SetActive(true);
            return;
        }

        GetObject((int)Objects.Object_BossUI).SetActive(false);
        GetObject((int)Objects.Object_MonsterUI).SetActive(true);
        curRemainEnemyNum = GameManager.Instance.Stage.StageData.MaxEnemySpawnNum;
        GetText((int)Texts.TXT_MonsterUI).text = ($"{GameManager.Instance.Stage.StageData.MaxEnemySpawnNum} / {GameManager.Instance.Stage.StageData.MaxEnemySpawnNum}");
    }

    // 스테이지 클리어 후 스테이지 이동 TXT
    public void UpdateStageClearEnemyUI()
    {
        GetObject((int)Objects.Object_MonsterUI).SetActive(true);
        GetText((int)Texts.TXT_MonsterUI).text = (" 스테이지 이동 중.. ");
        DeActivateBossSkillIndicator();
        GetObject((int)Objects.Object_BossUI).SetActive(false);      
    }

    // 몬스터 Count 업데이트
    public void UpdateEnemyCountUI()
    {
        curRemainEnemyNum--;
        if (curRemainEnemyNum < 0) curRemainEnemyNum = 0;
        GetText((int)Texts.TXT_MonsterUI).text = ($"{curRemainEnemyNum} / {GameManager.Instance.Stage.StageData.MaxEnemySpawnNum}");
    }

    // 보스 HPBar 업데이트
    public void UpdateBossHPBarUI(float healthPercentage)
    {
        if (bossHPBarSlider == null) 
            bossHPBarSlider = GetObject((int)Objects.Object_BossHpBarUI).GetComponent<Slider>(); 

        bossHPBarSlider.value = healthPercentage;
    }

    // 보스 Skill Indicator 활성화 & string Set
    public void ActivateBossSkillIndicator(string str)
    {
        GetObject((int)Objects.Object_BossSkillIndicator).SetActive(true);
        GetText((int)Texts.TXT_BossSkill).text = str;
    }

    // 보스 Skill Indicator 비활성화 
    public void DeActivateBossSkillIndicator()
    {
        if (GetObject((int)Objects.Object_BossSkillIndicator)!=null)
            GetObject((int)Objects.Object_BossSkillIndicator).SetActive(false);
    }

    // Water Boss 경고 TXT
    public void ActivateWBWarningTXT()
    {
        GetText((int)Texts.TXT_WBWarning).gameObject.SetActive(true);
        GetText((int)Texts.TXT_WBWarning).text = "! 해당 보스의 체력을 알 수 없습니다 !";
    }

    public void DeActivateWBWarningTXT()
    {
        GetText((int)Texts.TXT_WBWarning).gameObject.SetActive(false);
    }

    #endregion

    //스테이지값 업데이트
    public async void UpdateStageTXT(int stage)  
    {
        GetText((int)Texts.TXT_Stage).text = ($"STAGE  {stage}");
    }

    //소환 버튼 골드TXT 업데이트
    private void UpdateSummonGold()
    {
        GetText((int)Texts.TXT_SummonGold).text = PlayerDataManager.Instance.inGameData.UnitSpawnRequriedGold.ToString();
    }

    private void SummonOnClickEvent()
    {
        if (!PlayerDataManager.Instance.inGameData.IsUnitSpawn())
            return;

        BtnAnimaiton(Buttons.Btn_Summon);
        _unitSpawnManager.SpawnUnit();
        PlayerDataManager.Instance.inGameData.TakeSpawnUnitGold();
        UpdateSummonGold();
    }

    private void CheckSummonBtn(int amount)
    {
        if (PlayerDataManager.Instance.inGameData.IsUnitSpawn())
        {
            UnBlockSummonBtn();
            return;
        }
        BlockSummonBtn();
    }

    public bool CanSummon()
    {
        return GetButton((int)Buttons.Btn_Summon).interactable;
    }

    private async void SettingUIOnClickEvent()
    {
        BtnAnimaiton(Buttons.Btn_Settings);

        UI_Settings ui_Settings = await UIManager.Instance.ShowPopupUI<UI_Settings>(EUIRCode.UI_Settings);
        if(GameManager.Instance.Tutorial == null)
            ui_Settings.CheckScene(ESceneType.MainScene);
        else
            ui_Settings.CheckScene(ESceneType.TutorialScene);
    }
    
    private async void ReinforceOnClickEvent()
    {
        BtnAnimaiton(Buttons.Btn_Reinforce);
        
        await UIManager.Instance.ShowPopupUI<UI_Reinforce>(EUIRCode.UI_Reinforce);
    }
    
    private async void MythOnClickEvent()
    {
        BtnAnimaiton(Buttons.Btn_Myth);
        
        await UIManager.Instance.ShowPopupUI<UI_Myth>(EUIRCode.UI_Myth);
    }
    
    private async void GachaOnClickEvent()
    {
        BtnAnimaiton(Buttons.Btn_Gacha);
        
       UI_Gacha uI_Gacha =  await UIManager.Instance.ShowPopupUI<UI_Gacha>(EUIRCode.UI_Gacha);
        uI_Gacha.CheckGemStone();
    }
    
    private async void MissionOnClickEvent()
    {
        BtnAnimaiton(Buttons.Btn_Misson);
        
        await UIManager.Instance.ShowPopupUI<UI_Mission>(EUIRCode.UI_Mission);
    }
    
    private void AnimateButton(Button button)
    {
        Sequence buttonSequence = DOTween.Sequence();
        buttonSequence.Append(button.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad));
        buttonSequence.Append(button.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad));
    }

    //버튼 애니매이션
    public void BtnAnimaiton(Buttons buttonType)
    {
        Button clickedButton = GetButton((int)buttonType);

        if (!clickedButton.interactable)
            return;

        else
        {
            AnimateButton(clickedButton);
        }
    }
    

    public void BlockSummonBtn()
    {
        if (GetButton((int)Buttons.Btn_Summon) != null)
            GetButton((int)Buttons.Btn_Summon).interactable = false;
    }

    public void UnBlockSummonBtn()
    {
        if (GetButton((int)Buttons.Btn_Summon)!=null)
            GetButton((int)Buttons.Btn_Summon).interactable = true;
    }
}