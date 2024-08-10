using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UI_NameInput : UI_Popup
{
    [SerializeField] private TMP_InputField input; 
    [SerializeField] private Image warningImage;
    [SerializeField] private TextMeshProUGUI warningTXT;

    private readonly int minLength = 2;
    private readonly int maxLength = 7;

    private string curTXT;

    private void Start()
    {
        // 오브젝트 활성화 시 한 번 실행
        InitUI();
    }

    private void InitUI()
    {
        input.onEndEdit.AddListener(CheckInput);
        warningTXT.text = $"{minLength} ~ {maxLength} 글자 이내로 입력해주세요";
    }

    private void CheckInput(string name)
    {
        curTXT = name;
    }

    public void OnConfirmBtnClick()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);

        if (!IsValidLength())
            Warning();
        else
            // 설정 가능한 닉네임 길이면 이름 업데이트
            SetNickName(curTXT);
    }

    private bool IsValidLength()
    {
        if(curTXT == null) 
            return false;

        return curTXT.Length < maxLength && curTXT.Length >= minLength;
    }

    private void SetNickName(string name)
    {
        PlayerDataManager.Instance.SetPlayerData(name);
        UIManager.Instance.UI_StartScene.UpdatePlayerName(PlayerDataManager.Instance.SaveData.Name);
        // 한 번 실행되면 다시 실행 안되므로(안되나?) 파괴
        ClosePopupUI();
    }

    private void Warning()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnFail);

        // 이미 경고문구 켜져 있다면 return
        if (warningImage.gameObject.activeSelf)
            return;

        float duration = 2.5f;
        InitWarningUI();

        // 이미지와 텍스트의 투명도를 줄임
        warningImage.DOFade(0, duration).OnComplete(() => warningImage.gameObject.SetActive(false));
        warningTXT.DOFade(0, duration).OnComplete(() => warningTXT.gameObject.SetActive(false));
    }

    private void InitWarningUI()
    {
        warningImage.gameObject.SetActive(true);
        warningTXT.gameObject.SetActive(true);

        warningImage.DOFade(1, 0);
        warningTXT.DOFade(1, 0);
    }
}
