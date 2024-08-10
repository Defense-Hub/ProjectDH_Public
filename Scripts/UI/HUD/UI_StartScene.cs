using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
using UnityEngine;

public class UI_StartScene : UI_HUD
{
    private void Start()
    {
        UIManager.Instance.UI_StartScene = this;
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        GetButton((int)Buttons.Btn_StartSceneSetting)
            .AddOnClickEvent(async () => await OnClickSetting(Buttons.Btn_StartSceneSetting));
        GetButton((int)Buttons.Btn_Start).AddOnClickEvent(GameStartBtnOnClick);
        GetButton((int)Buttons.Btn_Quit).AddOnClickEvent(GameManager.Instance.GameQuit);
        
        if (PlayerDataManager.Instance.SaveData != null)
        {
            GetText((int)Texts.TXT_MaxStageVal).text = PlayerDataManager.Instance.SaveData.MaxStageLevel.ToString();
            UpdatePlayerName(PlayerDataManager.Instance.SaveData.Name);
        }
    }

    private void GameStartBtnOnClick()
    {
        if (PlayerDataManager.Instance.SaveData == null)
        {
            return;
        }

        GameManager.Instance.LoadScene.LoadMainScene();
    }

    public void UpdatePlayerName(string name)
    {
        GetText((int)Texts.TXT_PlayerName).text = name;
    }

    public async void ActiveNameInputField()
    {
        GameManager.Instance.StartScene.DeActivateTouchBlock();
        UI_NameInput ui = await UIManager.Instance.ShowPopupUI<UI_NameInput>(EUIRCode.UI_NameInput);
        ui.transform.SetParent(transform, false);
    }

    private async Task OnClickSetting(Buttons buttonType)
    {
        BtnAnimaiton(buttonType);
        UI_Settings ui_Settings = await UIManager.Instance.ShowPopupUI<UI_Settings>(EUIRCode.UI_Settings);
        ui_Settings.CheckScene(ESceneType.StartScene);
    }

    private void BtnAnimaiton(Buttons buttonType)
    {
        Button clickedButton = GetButton((int)buttonType);

        if (!clickedButton.interactable)
            return;

        else
        {
            AnimateButton(clickedButton);
        }
    }

    private void AnimateButton(Button button)
    {
        Sequence buttonSequence = DOTween.Sequence();
        buttonSequence.Append(button.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad));
        buttonSequence.Append(button.transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad));
    }
}