using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private float sceneMoveOffset;

    private Coroutine sceneLoadCoroutine;
    private WaitForSeconds wait;

    private void Start()
    {
        if (GameManager.Instance.LoadScene == null)
        {
            GameManager.Instance.LoadScene = this;
            wait = new WaitForSeconds(sceneMoveOffset);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!AddressableManager.Instance.IsLocationLoad)
        {
            SceneManager.LoadScene(ESceneType.TitleLoadingScene.ToString(), LoadSceneMode.Additive);
        }
    }

    public void LoadMainScene()
    {
        // UIManager.Instance.ResetPopUp();
        SoundManager.Instance.StopBgm();
        LoadBulidScene(ESceneType.MainScene);
        AnalyticsManager.Instance.AnalyticsPlayCount();
        SceneManager.LoadScene(ESceneType.InGameLoadingScene.ToString(), LoadSceneMode.Additive);
    }

    public void LoadBulidScene(ESceneType sceneType)
    {
        SceneManager.LoadScene(sceneType.ToString());
    }

    #region Start To Tutorial Scene Load

    public void LoadStartToTutorialScene()
    {
        if (sceneLoadCoroutine != null)
        {
            StopCoroutine(sceneLoadCoroutine);
        }

        sceneLoadCoroutine = StartCoroutine(DelayTransitionCoroutine(GoStartToTutorialCallback));
    }

    private void GoStartToTutorialCallback() // 첫 트랜지션 끝나고 호출되는 콜백 함수
    {
        SceneManager.UnloadSceneAsync(ESceneType.StartScene.ToString());
        SceneManager.UnloadSceneAsync(ESceneType.TitleLoadingScene.ToString());

        if (sceneLoadCoroutine != null)
        {
            StopCoroutine(sceneLoadCoroutine);
        }
        
        sceneLoadCoroutine = StartCoroutine(LoadTransitionSceneCoroutine
        (ESceneType.TutorialScene, BackStartToTutorialCallback, () => ResourceManager.Instance.curMapNum == 0));
    }


    private void BackStartToTutorialCallback() // 트랜지션이 끝나고 호출되는 콜백함수
    {
        GameManager.Instance.Tutorial.TutorialUIController.DialogueEvent.StartDialouge();
        SceneManager.UnloadSceneAsync(ESceneType.TransitionScene.ToString());
    }

    #endregion

    #region Tutorial To Start Scene Load
    
    [ContextMenu("T to S")]
    public void LoadTutorialToStartScene()
    {
        if (sceneLoadCoroutine != null)
        {
            StopCoroutine(sceneLoadCoroutine);
        }

        sceneLoadCoroutine = StartCoroutine(DelayTransitionCoroutine(GoTutorialToStartCallback));
    }

    private void GoTutorialToStartCallback()
    {
        // UIManager.Instance.ResetPopUp();
        ResourceManager.Instance.ResetUI();
        PlayerDataManager.Instance.ResetEvent();

        SceneManager.UnloadSceneAsync(ESceneType.TutorialScene.ToString());

        if (sceneLoadCoroutine != null)
        {
            StopCoroutine(sceneLoadCoroutine);
        }

        sceneLoadCoroutine = StartCoroutine(LoadTransitionSceneCoroutine
            (ESceneType.StartScene, BackTutorialToStartCallback, null));
    }

    private void BackTutorialToStartCallback()
    {
        SceneManager.UnloadSceneAsync(ESceneType.TransitionScene.ToString());
    }

    #endregion

    #region Main To Start Scene Load

    public void LoadMainToStartScene()
    {
        PlayerDataManager.Instance.SavePlayerData();

        if (sceneLoadCoroutine != null)
        {
            StopCoroutine(sceneLoadCoroutine);
        }

        sceneLoadCoroutine = StartCoroutine(DelayTransitionCoroutine(GoMainToStartCallback));
    }

    private void GoMainToStartCallback() // 트랜지션이 닫히고 나오는 콜백
    {
        Time.timeScale = 1f;

        // UIManager.Instance.ResetPopUp();
        PlayerDataManager.Instance.ResetEvent();
        SceneManager.UnloadSceneAsync(ESceneType.MainScene.ToString());
        ResourceManager.Instance.ResetUI();

        if (sceneLoadCoroutine != null)
        {
            StopCoroutine(sceneLoadCoroutine);
        }

        sceneLoadCoroutine = StartCoroutine(LoadTransitionSceneCoroutine
            (ESceneType.StartScene, BackMainToStartCallback, null));
    }

    private void BackMainToStartCallback() // 트랜지션이 열리고 나오는 콜백
    {
        SceneManager.UnloadSceneAsync(ESceneType.TransitionScene.ToString());

        if (GameManager.Instance.StartScene != null)
        {
            GameManager.Instance.StartScene.DeActivateTouchBlock();
        }
    }

    #endregion

    private IEnumerator DelayTransitionCoroutine(UnityAction startCallback) // 트랜지션 씬이 호출 될 때까지 기다림
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(ESceneType.TransitionScene.ToString(), LoadSceneMode.Additive);

        yield return op;

        GameManager.Instance.Transition.StartTransition(startCallback);
    }

    private IEnumerator LoadTransitionSceneCoroutine(ESceneType loadScene, UnityAction endCallback, Func<bool> isDelay) // 씬 로드
    {
        SceneManager.LoadSceneAsync(loadScene.ToString(), LoadSceneMode.Additive);


        yield return wait;

        switch (loadScene)
        {
            case ESceneType.TutorialScene:
                GameManager.Instance.Tutorial.TutorialInit();
                break;
            case ESceneType.StartScene:
                GameManager.Instance.StartScene.StartSceneInit();
                break;
        }
       
        if (isDelay != null)
        {
            while (isDelay.Invoke())
            {
                yield return null;
            }
        }
        
        yield return wait;

        GameManager.Instance.Transition.EndTransition(endCallback);
    }
}