using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SummonPopup : UI_Popup
{
    [SerializeField] private TextMeshProUGUI summonUnitRankText;
    [SerializeField] private TextMeshProUGUI summonUnitText;
    //[SerializeField] private TextMeshProUGUI summonUnitProbabilityText;
    [SerializeField] private Button backgroundBtn;

    [SerializeField] private Color UniqueTextColor;
    [SerializeField] private Color LegendaryTextColor;
    [SerializeField] private Color EpicTextColor;

    [SerializeField] private float popupDelayTime = 1f;

    private EUnitRank curUnitRank;
    private string curUnitName;
    // TODO : Unit 썸네일?

    public void InitSummonUnitData(UnitData unitData)
    {
        curUnitName = unitData.Name;
        curUnitRank = unitData.UnitRank;
    }

    public override void Init()
    {
        base.Init();
        InitTextColor();
        summonUnitRankText.text = $"{curUnitRank}";
        summonUnitText.text = $"{curUnitName} 획득!";

        ClosePopupDelayUI(popupDelayTime);
        backgroundBtn.onClick.RemoveAllListeners();
        backgroundBtn.onClick.AddListener(ClosePopupUI);
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    public override void ClosePopupDelayUI(float delayTime)
    {
        base.ClosePopupDelayUI(delayTime);
    }

    private void InitTextColor()
    {
        switch (curUnitRank)
        {
            case EUnitRank.Unique:
                summonUnitRankText.color = UniqueTextColor;
                break;

            case EUnitRank.Legendary:
                summonUnitRankText.color = LegendaryTextColor;
                break;

            case EUnitRank.Epic:
                summonUnitRankText.color = EpicTextColor;
                break;

            default :
                break;
        }
    }
}
