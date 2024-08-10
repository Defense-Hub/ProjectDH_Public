using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class UI_Reinforce : UI_Popup
{
    [SerializeField] private GameObject TXT_MaxNormal;
    [SerializeField] private GameObject TXT_MaxAdvanced;
    [SerializeField] private GameObject TXT_MaxSpawnProbability;

    private PlayerInGameData inGameData;

    private void Start()
    {
        inGameData = PlayerDataManager.Instance.inGameData;
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        GetText((int)Texts.TXT_Reinforce_Gold).text = PlayerDataManager.Instance.inGameData.Gold.ToString();
        GetText((int)Texts.TXT_Reinforce_GemStone).text = PlayerDataManager.Instance.inGameData.GemStone.ToString();

        GetText((int)Texts.TXT_Normal_Gold).text = GameManager.Instance.System.Enforce.GetBasicEnforceRequiredCurrency.ToString();
        GetText((int)Texts.TXT_Unique_Gold).text = GameManager.Instance.System.Enforce.GetAdvanceEnforceRequiredCurrency.ToString();
        GetText((int)Texts.TXT_SummonRate_Gold).text = GameDataManager.Instance.GameCurrencyData.SpawnProbabilityEnforceGold.ToString();
        
        GetButton((int)Buttons.Btn_Normal_Reinforce).AddOnClickEvent(OnClickBasicEnforce);
        GetButton((int)Buttons.Btn_Unique_Reinforce).AddOnClickEvent(OnClickAdvancedEnforce);
        GetButton((int)Buttons.Btn_SummonRate_Reinforce).AddOnClickEvent(OnClickSpawnProbability);
        GetButton((int)Buttons.Btn_Reinforce_Close).AddOnClickEvent(ClosePopupUI);

        PlayerDataManager.Instance.inGameData.OnChangeGold += UpdateGoldUI;
        PlayerDataManager.Instance.inGameData.OnChangeGemStone += UpdateGemStoneUI;

        CheckGold();
        CheckGemStone();
    }

    #region 버튼 상태 체크 메서드
    // Basic 버튼 체크
    private bool CanEnforceBasic()
    {
        // Max 레벨에 달성하면
        if (PlayerDataManager.Instance.inGameData.EnhancementData.basicEnhancementLevel >= GameDataManager.Instance.BasicUnitEnforceDatas.Count)
        {
            GetText((int)Texts.TXT_Normal_Lv).text = "LV.MAX";
            GetButton((int)Buttons.Btn_Normal_Reinforce).interactable = false;

            if (GetText((int)Texts.TXT_Normal_Gold) != null)
                GetText((int)Texts.TXT_Normal_Gold).gameObject.SetActive(false);
            TXT_MaxNormal.SetActive(true);

            return false;
        }

        // 재화가 충분하지 않으면
        if (!GameManager.Instance.System.HasEnoughBasicGold())
        {
            GetButton((int)Buttons.Btn_Normal_Reinforce).interactable = false;
            return false;
        }

        GetButton((int)Buttons.Btn_Normal_Reinforce).interactable = true;
        return true;
    }

    // Advanced 버튼 체크
    private bool CanEnforceAdvanced()
    {
        if (PlayerDataManager.Instance.inGameData.EnhancementData.advancedEnhancementLevel >= GameDataManager.Instance.AdvancedUnitEnforceDatas.Count)
        {
            GetText((int)Texts.TXT_Unique_Lv).text = "LV.MAX";
            GetButton((int)Buttons.Btn_Unique_Reinforce).interactable = false;

            if (GetText((int)Texts.TXT_Unique_Gold) != null)
                GetText((int)Texts.TXT_Unique_Gold).gameObject.SetActive(false);
            TXT_MaxAdvanced.SetActive(true);
            return false;
        }

        if (!GameManager.Instance.System.HasEnoughAdvancedGem())
        { 
            GetButton((int)Buttons.Btn_Unique_Reinforce).interactable = false;
            return false;
        }

        GetButton((int)Buttons.Btn_Unique_Reinforce).interactable = true;
        return true;
    }

    // Spawn Probability 버튼 체크
    private bool CanEnforceSpawnProbability()
    {
        if (inGameData.SpawnProbabilityLevel >= GameDataManager.Instance.SpawnProbability.Count)
        {
            GetText((int)Texts.TXT_SummonRate_Lv).text = "LV.MAX";
            GetButton((int)Buttons.Btn_SummonRate_Reinforce).interactable = false;

            if (GetText((int)Texts.TXT_SummonRate_Gold) != null)
                GetText((int)Texts.TXT_SummonRate_Gold).gameObject.SetActive(false);
            TXT_MaxSpawnProbability.SetActive(true);
            return false;
        }

        if (!GameManager.Instance.System.Enforce.CanEnforceSpawnProbability())
        {
            GetButton((int)Buttons.Btn_SummonRate_Reinforce).interactable = false;
            return false;
        }

        GetButton((int)Buttons.Btn_SummonRate_Reinforce).interactable = true;
        return true;
    }
    #endregion

    #region 버튼 온클릭 메서드
    private void OnClickBasicEnforce()
    {
        if (!CanEnforceBasic())
            return;

        // 강화 가능하다면
        GameManager.Instance.System.TryEnforceBasicUnit();

        GetText((int)Texts.TXT_Normal_Gold).text = GameManager.Instance.System.Enforce.GetBasicEnforceRequiredCurrency.ToString();
        GetText((int)Texts.TXT_Normal_Lv).text = ($"LV.{inGameData.EnhancementData.basicEnhancementLevel}");
        CheckGold();
    }

    private void OnClickAdvancedEnforce()
    {
        if (!CanEnforceAdvanced())
            return;

        // 강화 가능하다면
        GameManager.Instance.System.TryEnforceAdvancedUnit();
        
        GetText((int)Texts.TXT_Unique_Gold).text = GameManager.Instance.System.Enforce.GetAdvanceEnforceRequiredCurrency.ToString();
        GetText((int)Texts.TXT_Unique_Lv).text = ($"LV.{inGameData.EnhancementData.advancedEnhancementLevel}");
        CheckGemStone();
    }

    private void OnClickSpawnProbability()
    {
        if (!CanEnforceSpawnProbability())
            return;

        // 강화 가능하다면
        GameManager.Instance.System.Enforce.EnforceSpawnProbability();

        GetText((int)Texts.TXT_Reinforce_Gold).text = inGameData.Gold.ToString();
        GetText((int)Texts.TXT_SummonRate_Lv).text = ($"LV.{inGameData.SpawnProbabilityLevel}");
        CheckGold();
    }
    #endregion

    private void UpdateGoldUI(int amount)
    {
        GetText((int)Texts.TXT_Reinforce_Gold).text = amount.ToString();
        CheckGold();
    }

    private void UpdateGemStoneUI(int amount)
    {
        GetText((int)Texts.TXT_Reinforce_GemStone).text = amount.ToString();
        CheckGemStone();
    }

    private void CheckGold()
    {
        CanEnforceBasic();
        CanEnforceSpawnProbability();
    }

    private void CheckGemStone()
    {
        CanEnforceAdvanced();
    }
}
