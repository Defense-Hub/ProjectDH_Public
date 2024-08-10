//using System.Diagnostics;
using System.Collections;
using TMPro;
using UnityEngine;


public class UI_StageEffect : UI_Popup
{
    enum Texts
    {
        TXT_Stage
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TMP_Text>(typeof(Texts));      

    }
    public void PopupStage(int Stage)
    {
        StartCoroutine(ShowPopup(Stage));
    }

    private IEnumerator ShowPopup(int Stage)
    {
        yield return null;

        Debug.Log(GetText((int)Texts.TXT_Stage));
        GetText((int)Texts.TXT_Stage).text = Stage.ToString();
        ClosePopupDelayUI(2f);
    }
}
