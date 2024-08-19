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

    private string curInput;

    private void Start()
    {
        // 오브젝트 활성화 시 한 번 실행
        InitUI();
    }

    private void InitUI()
    {
        // 커서 활성화
        ActivateCursor("");
        // input field 선택될 때마다 커서 활성화
        input.onSelect.AddListener(ActivateCursor);
        // 빈 칸이 입력되는지 확인
        input.onValueChanged.AddListener(CheckBlank);
        // 입력 끝나면 curInput 업데이트
        input.onEndEdit.AddListener(UpdateInput);
        warningTXT.text = $"{minLength} ~ {maxLength} 글자 이내로 입력해주세요";
    }

    private void ActivateCursor(string name)
    {
        input.ActivateInputField();
    }

    private void CheckBlank(string name)
    {
        string newText = name.Replace(" ", "");
        if (name != newText)
        {
            input.text = newText;
        }
    }

    private void UpdateInput(string name)
    {
        curInput = name;
    }

    public void OnConfirmBtnClick()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);

        if (!IsValidLength())
            Warning();
        else
            // 설정 가능한 닉네임 길이면 이름 업데이트
            SetNickName(curInput);
    }

    private bool IsValidLength()
    {
        if(curInput == null) 
            return false;

        return curInput.Length < maxLength && curInput.Length >= minLength;
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
