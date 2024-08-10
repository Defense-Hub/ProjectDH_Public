using UnityEngine;
using UnityEngine.UI;

public class UI_GameClear : UI_Popup
{
    [SerializeField] private Button mainMenuBtn;

    public override void Init()
    {
        base.Init();
        GameManager.Instance.System.SetGameSpeed(0);

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayUISfx(EUISfx.GameClear);

        mainMenuBtn.onClick.RemoveAllListeners();
        mainMenuBtn.onClick.AddListener(LoadStartScene);
    }

    private void LoadStartScene()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);
        GameManager.Instance.LoadScene.LoadMainToStartScene();
    }
}
