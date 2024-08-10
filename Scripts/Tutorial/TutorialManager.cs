using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    [field:Header("# Tutorial Info")]
    [field:SerializeField] public TutorialUIController TutorialUIController { get; private set; }
    
    [Header("# TestMode")]
    public bool testMode;


    private void Start()
    {
        GameManager.Instance.Tutorial = this;
        
        if (testMode)
        {
            // SceneManager.LoadScene(ESceneType.TitleLoadingScene.ToString(), LoadSceneMode.Additive);
            StartCoroutine(TutorialStartCouroutine());
        }
        else
        {
            // TutorialInit();
        }
    }

    public void TutorialInit()
    {
        SetTutorialData();
        SoundManager.Instance.PlayBgm(EBgm.Main);
    }
    
    private IEnumerator TutorialStartCouroutine()
    {
        yield return new WaitForSeconds(10f);
        TutorialInit();
    }

    [ContextMenu("Load")]
    private async void SetTutorialData()
    {
        // 맵, 유닛, 
        PlayerDataManager.Instance.CreateInGameData();
        GameManager.Instance.System.SetGameSystem();
        
        await ResourceManager.Instance.SetPoolData();
        
        PlayerDataManager.Instance.CallPlayerDataUISetting();
        GameManager.Instance.Stage.InitStageData();
        TutorialUIController.TutorialInit();
    }
}
