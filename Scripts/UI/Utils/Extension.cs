using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Extension
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.AddUIEvent(go, action, type);
    }
    
    public static void AddOnClickEvent(this Button btn, UnityAction onClickEvent)
    {
        btn.onClick.AddListener(onClickEvent);
        btn.onClick.AddListener(() => SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick));
    }
}
