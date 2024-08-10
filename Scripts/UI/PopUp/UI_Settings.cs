using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : UI_Popup
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxUISlider;
    [SerializeField] private Slider sfxInGameSlider;

    enum Buttons
    {
        Btn_Close,
        Btn_1X,
        Btn_2X,
        Btn_MainToLobby,
        Btn_TutorialToLobby
    }

    enum GameObjects
    {
        Obj_GameSpeed,
        Obj_MainToLobby,
        Obj_TutorialToLobby
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.Btn_Close).AddOnClickEvent(ClosePopupUI);
        GetButton((int)Buttons.Btn_1X).AddOnClickEvent(() => GameManager.Instance.System.SetGameSpeed(1));
        GetButton((int)Buttons.Btn_2X).AddOnClickEvent(() => GameManager.Instance.System.SetGameSpeed(2));
        GetButton((int)Buttons.Btn_MainToLobby).AddOnClickEvent(() =>
        {
            SoundManager.Instance.StopBgm();
            GameManager.Instance.LoadScene.LoadMainToStartScene();
        });
        GetButton((int)Buttons.Btn_TutorialToLobby).AddOnClickEvent(() =>
        {
            SoundManager.Instance.StopBgm();
            GameManager.Instance.LoadScene.LoadTutorialToStartScene();
        });


        if (masterSlider != null)
        {
            masterSlider.onValueChanged?.AddListener(UpdateMasterSound);
            masterSlider.value = SoundManager.Instance.GetAudioVolume(EAudioMixerType.Master);
        }
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged?.AddListener(UpdateBgm);
            bgmSlider.value = SoundManager.Instance.GetAudioVolume(EAudioMixerType.Bgm);
        }
           
        if (sfxUISlider != null)
        {
            sfxUISlider.onValueChanged?.AddListener(UpdateSfxUI);
            sfxUISlider.value = SoundManager.Instance.GetAudioVolume(EAudioMixerType.SfxUI);
        }
            
        if (sfxInGameSlider != null)
        {
            sfxInGameSlider.onValueChanged?.AddListener(UpdateSfxInGame);
            sfxInGameSlider.value = SoundManager.Instance.GetAudioVolume(EAudioMixerType.SfxInGame);
        }           
    }

    public void CheckScene(ESceneType eSceneType)
    {
        switch (eSceneType)
        {
            case ESceneType.StartScene:
                LobbySceneSetting();
                break;
            case ESceneType.MainScene:
                GameSceneSetting();
                break;
            case ESceneType.TutorialScene:
                TutorialSceneSetting();
                break;
        }
    }

    // 로비 - 사운드만
    public void LobbySceneSetting()
    {
        GetObject((int)GameObjects.Obj_GameSpeed).SetActive(false);
        GetObject((int)GameObjects.Obj_MainToLobby).gameObject.SetActive(false);
        GetObject((int)GameObjects.Obj_TutorialToLobby).gameObject.SetActive(false);
    }

    // 게임씬  - 사운드 + 배속 + 로비버튼
    public void GameSceneSetting()
    {
        GetObject((int)GameObjects.Obj_GameSpeed).SetActive(true);
        GetObject((int)GameObjects.Obj_MainToLobby).gameObject.SetActive(true);
        GetObject((int)GameObjects.Obj_TutorialToLobby).gameObject.SetActive(false);
    }

    // 튜토리얼씬 - 사운드 + 로비버튼
    public void TutorialSceneSetting()
    {
        GetObject((int)GameObjects.Obj_GameSpeed).SetActive(false);
        GetObject((int)GameObjects.Obj_MainToLobby).gameObject.SetActive(false);

        // 두번째 튜토리얼 부터만 로비로 돌아가기 버튼 활성화
        if (PlayerDataManager.Instance.SaveData == null)
            GetObject((int)GameObjects.Obj_TutorialToLobby).gameObject.SetActive(false);
        else
            GetObject((int)GameObjects.Obj_TutorialToLobby).gameObject.SetActive(true);
    }

    #region 사운드 업데이트
    public void UpdateMasterSound(float value)
    {
        if (value == 0)
        {
            SoundManager.Instance.SetAudioMute(EAudioMixerType.Master);
            return;
        }
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.Master, value);
    }

    public void UpdateBgm(float value)
    {
        if (value == 0)
        {
            SoundManager.Instance.SetAudioMute(EAudioMixerType.Bgm);
            return;
        }
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.Bgm, value);
    }

    public void UpdateSfxUI(float value)
    {
        if (value == 0)
        {
            SoundManager.Instance.SetAudioMute(EAudioMixerType.SfxUI);
            return;
        }
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.SfxUI, value);
    }

    public void UpdateSfxInGame(float value)
    {
        if (value == 0)
        {
            SoundManager.Instance.SetAudioMute(EAudioMixerType.SfxInGame);
            return;
        }
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.SfxInGame, value);
    }
    #endregion
}
