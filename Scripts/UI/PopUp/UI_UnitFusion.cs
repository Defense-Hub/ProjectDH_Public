using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitFusion_Back : UI_Popup
{
    //enum
    enum Buttons
    {
        Btn_Close,
        Btn_Summon
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
        Bind<Image>(typeof(EImages));

        GetButton((int)Buttons.Btn_Close).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.Btn_Close, data));
        GetButton((int)Buttons.Btn_Summon).gameObject.AddUIEvent((PointerEventData data) => OnButtonClicked(Buttons.Btn_Summon, data));
    }

    private void OnButtonClicked(Buttons buttonType, PointerEventData data)
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);

        switch (buttonType)
        {
            case Buttons.Btn_Close:
                ClosePopupUI();
                break;

            case Buttons.Btn_Summon:
                break;

        }
    }

    private float CalculateAttackPowerIncrease()
    {
        // 공격력 증가량 계산 로직 추가
        return 0f;
    }

    private float CalculateAttackSpeedIncrease()
    {
        // 공격속도 증가량 계산 로직 추가
        return 0f;
    }
}