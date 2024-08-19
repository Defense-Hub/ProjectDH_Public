using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Gacha : UI_Popup
{
    private int normalReinforceValue;
    private int uniqueReinforceValue;
    private int legendaryReinforceValue;

    private Sprite Origin_Normal_Icon;
    private Sprite Origin_Unique_Icon;
    private Sprite Origin_Legendary_Icon;


    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(EImages));

        GetText((int)Texts.TXT_Gacha_GemStone).text = PlayerDataManager.Instance.inGameData.GemStone.ToString();

        GetButton((int)Buttons.Btn_Normal).AddOnClickEvent(NormalGachaUpdate);
        GetButton((int)Buttons.Btn_Unique).AddOnClickEvent(UniqueGachaUpdate);
        GetButton((int)Buttons.Btn_Legendary).AddOnClickEvent(LegendryGachaUpdate);
        GetButton((int)Buttons.Btn_Gacha_Close).AddOnClickEvent(ClosePopupUI);
        
        Origin_Normal_Icon = GetImage((int)EImages.Image_Normal_Icon).sprite;
        Origin_Unique_Icon = GetImage((int)EImages.Image_Unique_Icon).sprite;
        Origin_Legendary_Icon = GetImage((int)EImages.Image_Legendary_Icon).sprite;

        normalReinforceValue = GameDataManager.Instance.GambleProbabilityDatas[0].RequiredGemStone;
        uniqueReinforceValue = GameDataManager.Instance.GambleProbabilityDatas[1].RequiredGemStone;
        legendaryReinforceValue = GameDataManager.Instance.GambleProbabilityDatas[2].RequiredGemStone;

        CheckGemStone();

        PlayerDataManager.Instance.inGameData.OnChangeGemStone += UpdateGemStoneText;

        GameManager.Instance.UnitSpawn.Controller.OnUnBlockSummonBtn += OnUnBlockGachaBtn;
    }

    private void UpdateGemStoneText(int amount)
    {
        GetText((int)Texts.TXT_Gacha_GemStone).text = amount.ToString();
        CheckGemStone();
    }

    private void LegendryGachaUpdate()
    {
        UpdateGacha(Texts.TXT_Legendary,  legendaryReinforceValue);
    }
    
    private void UniqueGachaUpdate()
    {
        UpdateGacha(Texts.TXT_Unique,uniqueReinforceValue);
    }
    
    private void NormalGachaUpdate()
    {
        UpdateGacha(Texts.TXT_Normal,normalReinforceValue);
    }

    private void UpdateGacha(Texts goldTextType,  int reinforceValue)
    {
        GetText((int)goldTextType).text = reinforceValue.ToString();
        
        switch (goldTextType)
        {
            case Texts.TXT_Normal:
                StartGamble(EGachaType.Common, EImages.Image_Normal_Icon);
                break;
            
            case Texts.TXT_Unique:
                StartGamble(EGachaType.Unique, EImages.Image_Unique_Icon);
                break;
            
            case Texts.TXT_Legendary:
                StartGamble(EGachaType.Legendary, EImages.Image_Legendary_Icon);
                break;
        }

        CheckGemStone(); // 갱신된 강화 값에 따라 버튼 상태 업데이트

        GetText((int)Texts.TXT_Gacha_GemStone).text = PlayerDataManager.Instance.inGameData.GemStone.ToString();
    }

    private void StartGamble(EGachaType gachaType, EImages iconType)
    {
        int GachaResultId = GameManager.Instance.System.SelectGamble((int)gachaType);
        int icon = (int)iconType;
        Vector3 targetPos = GetImage(icon).gameObject.transform.position;
        if (GachaResultId > 0)
            GetImage(icon).sprite = GameDataManager.Instance.UnitBases[GachaResultId].Thumbnail;
                
        ResultGacha(GachaResultId, targetPos);
    }
    private void ResultGacha(int resultId, Vector3 startPos)
    {
        float targetY;
        UI_GachaResult resultText;
        if(resultId >= 0)
        {
            resultText = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_GachaResult).ReturnMyComponent<UI_GachaResult>();
            resultText.Text.transform.position = startPos + Vector3.up * 15f;
            resultText.Text.text = "성공";

            // 닷트윈 
            targetY = resultText.Text.transform.position.y + 10f;
            resultText.Text.transform.DOMoveY(targetY, 1f)
                .SetAutoKill(true)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    switch(resultId / 100)
                    {
                        case 1:
                            GetImage((int)EImages.Image_Normal_Icon).sprite = Origin_Normal_Icon;
                            break;
                        case 2:
                            GetImage((int)EImages.Image_Unique_Icon).sprite = Origin_Unique_Icon;
                            break;
                        case 3:
                            GetImage((int)EImages.Image_Legendary_Icon).sprite = Origin_Legendary_Icon;
                            break;
                    }
                    resultText.gameObject.SetActive(false);
                    });

            GameManager.Instance.UnitSpawn.TargetUnitSpawn(resultId);
        }
        else
        {
            switch (resultId)
            {
                case (int)EGachaResult.NoSpace:
                    // TODO : 공간이 부족할 때
                    break;

                case (int)EGachaResult.NoMoney:
                    // TODO : 돈이 없을 때
                    break;

                case (int)EGachaResult.Fail:
                    resultText = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_GachaResult).ReturnMyComponent<UI_GachaResult>();
                    resultText.Text.transform.position = startPos + Vector3.up * 15f;
                    resultText.Text.text = "실패";

                    // 닷트윈 
                    // 진행 중에 새로운 입력이 들어올 경우 고장...
                    targetY = resultText.Text.transform.position.y + 10f;
                    resultText.Text.transform.DOMoveY(targetY, 1f)
                        .SetAutoKill(false)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() => resultText.gameObject.SetActive(false));

                    break;
            }
        }
    }

    public void CheckGemStone()
    {
        UpdateButtonAndText(Buttons.Btn_Normal, normalReinforceValue);
        UpdateButtonAndText(Buttons.Btn_Unique, uniqueReinforceValue);
        UpdateButtonAndText(Buttons.Btn_Legendary, legendaryReinforceValue);
    }

    private void UpdateButtonAndText(Buttons buttonType, int reinforceValue)
    {
        bool hasEnoughGemStones = PlayerDataManager.Instance.inGameData.HasGemStone(reinforceValue);
        GetButton((int)buttonType).interactable = hasEnoughGemStones && !GameManager.Instance.UnitSpawn.Controller.IsFullTile;
    }

    private void OnUnBlockGachaBtn(bool isUnBlock)
    {
        if (GetButton((int)Buttons.Btn_Normal) != null)
            GetButton((int)Buttons.Btn_Normal).interactable = isUnBlock;
        
        if (GetButton((int)Buttons.Btn_Unique) != null)
            GetButton((int)Buttons.Btn_Unique).interactable = isUnBlock;
        
        if (GetButton((int)Buttons.Btn_Legendary) != null)
            GetButton((int)Buttons.Btn_Legendary).interactable = isUnBlock;
    }
}
