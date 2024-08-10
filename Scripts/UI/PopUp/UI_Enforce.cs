using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Enforce : UI_Popup
{
    enum Buttons
    {
        CloseBtn,
        CtrlUpBtn,
        FasterBtn,
        LifeUpBtn
    }

    enum Texts
    {
        Tittle,
        CtrlUpText,
        FasterText,
        LifeUpText
    }

    enum GameObjects
    {
        EnforceBtns
    }

    enum Images
    {
        Blocker,
        Panul,
        CloseIcon,
        CtrlUpIcon,
        FasterIcon,
        LifeUpIcon
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.CloseBtn).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.CloseBtn, data));
        GetButton((int)Buttons.CtrlUpBtn).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.CtrlUpBtn, data));
        GetButton((int)Buttons.FasterBtn).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.FasterBtn, data));
        GetButton((int)Buttons.LifeUpBtn).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.LifeUpBtn, data));
    }

    private void OnButtonClicked(Buttons buttonType, PointerEventData data)
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);

        switch (buttonType)
        {
            case Buttons.CloseBtn:
                // UIManager.Instance.ClosePopupUI(this);
                break;
            case Buttons.CtrlUpBtn:
                IncreaseTextValue(Texts.CtrlUpText);
                break;
            case Buttons.FasterBtn:
                IncreaseTextValue(Texts.FasterText);
                break;
            case Buttons.LifeUpBtn:
                IncreaseTextValue(Texts.LifeUpText);
                break;
        }
    }

    private void IncreaseTextValue(Texts textType)
    {
        TMP_Text text = Get<TMP_Text>((int)textType);
        if (text != null)
        {
            int value;
            if (int.TryParse(text.text, out value))
            {
                value++;
                text.text = value.ToString();
            }
        }
    }
}
