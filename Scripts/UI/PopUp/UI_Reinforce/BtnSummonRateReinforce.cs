using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnSummonRateReinforce : MonoBehaviour
{
    private Button button;
    private TMP_Text goldText;
    private PlayerInGameData inGameData;

    private int reinforceValue;

    private void Start()
    {
        button = GetComponent<Button>();
        goldText = GetComponentInChildren<TMP_Text>();

        inGameData = PlayerDataManager.Instance.inGameData;

        reinforceValue = inGameData.GemStone;
        UpdateGoldText();

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);
        reinforceValue += 2;
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldText.text = reinforceValue.ToString();
    }
}
