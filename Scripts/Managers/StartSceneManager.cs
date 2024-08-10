using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject touchBlock;
    
    private void Start()
    {
        GameManager.Instance.StartScene = this;
    }

    public void StartSceneInit()
    {
        SoundManager.Instance.PlayBgm(EBgm.Lobby);
        //UIManager.Instance.UI_StartScene.Init();
        // TODO :: 스타트씬 Init 내용 작성
        UIManager.Instance.UI_StartScene.Init();
        if (PlayerDataManager.Instance.SaveData == null)
        {
            UIManager.Instance.UI_StartScene.ActiveNameInputField();
        }
    }

    public void DeActivateTouchBlock()
    {
        touchBlock.SetActive(false);
    }
}
