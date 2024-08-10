using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : UI_Popup
{
    [SerializeField] private TextMeshProUGUI curStageText;
    [SerializeField] private Button mainMenuBtn;

    public override void Init()
    {
        base.Init();
        GameManager.Instance.System.SetGameSpeed(0);

        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayUISfx(EUISfx.GameOver);

        curStageText.text = $"현재 스테이지 : {GameManager.Instance.Stage.CurStageLevel}";
        mainMenuBtn.onClick.RemoveAllListeners();
        mainMenuBtn.onClick.AddListener(LoadStartScene);
    }

    private void LoadStartScene()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);
        GameManager.Instance.LoadScene.LoadMainToStartScene();
    }
}
