using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDataSetSystem : MonoBehaviour
{
    [Header("Loading Image")]
    [SerializeField] private Image progressBar;
    [SerializeField] private Image wormo;

    [Header("Increase Speed")]
    [SerializeField] private float increaseSpeed = 0.2f;

    private Coroutine delayCoroutine;
    private RectTransform parentRect;
    private float progressBarWidth;

    private void Start()
    {
        SetGameData();
        InitProgressBar();
    }
    
    private async void SetGameData()
    {
        PlayerDataManager.Instance.CreateInGameData();
        GameManager.Instance.System.SetGameSystem();
        await ResourceManager.Instance.SetPoolData();

        GameManager.Instance.Stage.InitStageData();
        
        progressBar.fillAmount = 0.3f;
        GetWormoPosition();

        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }

        delayCoroutine = StartCoroutine(DataSetCoroutine());      
    }
    
    private IEnumerator DataSetCoroutine()
    {       
        while (progressBar.fillAmount < 1f)
        {
            progressBar.fillAmount += Time.deltaTime * increaseSpeed;
            GetWormoPosition();
            yield return null;
        }

        PlayerDataManager.Instance.CallPlayerDataUISetting();
        
        SceneManager.UnloadSceneAsync(ESceneType.InGameLoadingScene.ToString());
        GameManager.Instance.Stage.GameStart();
    }

    private void InitProgressBar()
    {
        progressBar.fillAmount = 0f;
        parentRect = progressBar.transform.parent.GetComponent<RectTransform>();

        // ProgressBar 실제 너비
        progressBarWidth = progressBar.rectTransform.rect.width * parentRect.lossyScale.x;
    }

    private void GetWormoPosition()
    {
        // FillAmount에 따른 좌표
        float nowProgressBarPos = progressBar.fillAmount * progressBarWidth;

        wormo.transform.position = new Vector3(nowProgressBarPos + 100f, wormo.transform.position.y, wormo.transform.position.z);
    }
}
