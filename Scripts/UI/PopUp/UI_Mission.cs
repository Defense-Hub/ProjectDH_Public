using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Mission : UI_Popup
{
    enum Buttons
    {
        Btn_Close,
        Btn_Block
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Btn_Close).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.Btn_Close, data));
        GetButton((int)Buttons.Btn_Block).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.Btn_Block, data));
    }

    private void OnButtonClicked(Buttons buttonType, PointerEventData data)
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);

        switch (buttonType)
        {
            case Buttons.Btn_Close:
                ClosePopupUI();
                break;
            case Buttons.Btn_Block:
                ClosePopupUI();
                break;
        }
    }
}
