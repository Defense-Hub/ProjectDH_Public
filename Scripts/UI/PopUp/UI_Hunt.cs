using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Hunt : UI_Popup
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Btn_Hunt).AddOnClickEvent(OnHuntInfo);
    }

    private async void OnHuntInfo()
    {
        await UIManager.Instance.ShowPopupUI<UI_HuntInfo>(EUIRCode.UI_HuntInfo);
    }

}
