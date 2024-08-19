using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameDataLoadSystem : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private float increaseSpeed = 0.2f;

    private Coroutine delayCoroutine;

    private void Awake()
    {
        progressBar = GameObject.FindGameObjectWithTag("ProgressBar").GetComponent<Image>();
        progressBar.fillAmount = 0f;
    }

    private void Start()
    {
        InitDataLoad();
    }

    private async void MainSceneTestInit()
    {
        PlayerDataManager.Instance.CreateInGameData();
        GameManager.Instance.System.SetGameSystem();
        await ResourceManager.Instance.SetPoolData();
        GameManager.Instance.Stage.InitStageData();
        PlayerDataManager.Instance.CallPlayerDataUISetting();
    }
    private async void InitDataLoad()
    {
        await AddressableManager.Instance.GetLocations();

        progressBar.fillAmount = 0.3f;

        await ResourceManager.Instance.LoadAddressableDatas();

        progressBar.fillAmount = 0.7f;

        PlayerDataManager.Instance.LoadPlayerData();
        await GameDataManager.Instance.LoadGameData();

        progressBar.fillAmount = 0.8f;

        if (GameManager.Instance.isMainSceneTest)
        {
            MainSceneTestInit();
        }

        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }

        delayCoroutine = StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        while (progressBar.fillAmount < 1f)
        {
            progressBar.fillAmount += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        if (GameManager.Instance.isMainSceneTest)
            GameManager.Instance.Stage.GameStart();
        else
        {
            if (PlayerDataManager.Instance.SaveData == null && GameManager.Instance.Tutorial == null) // 세이브 데이터가 없을 떄,
            {
                if (GameManager.Instance.isStartSceneTest) // 스타트 씬 테스트일 떄
                {
                    GameManager.Instance.StartScene.StartSceneInit();
                }
                else
                {
                    GameManager.Instance.LoadScene.LoadStartToTutorialScene(); // 튜토리얼 씬 로드
                    yield break;
                }
            }
            else
            {
                if (PlayerDataManager.Instance.SaveData == null)
                {
                    UIManager.Instance.UI_StartScene.ActiveNameInputField();
                }
                else
                {
                    if (GameManager.Instance.StartScene != null)
                    {
                        GameManager.Instance.StartScene.StartSceneInit();
                        UIManager.Instance.UI_StartScene.UpdatePlayerName(PlayerDataManager.Instance.SaveData.Name);
                        GameManager.Instance.StartScene.DeActivateTouchBlock();
                    }
                    else
                    {
                        Debug.Log("StartScene Null");
                    }
                }
            }
        }

        SceneManager.UnloadSceneAsync(ESceneType.TitleLoadingScene.ToString());
    }
}