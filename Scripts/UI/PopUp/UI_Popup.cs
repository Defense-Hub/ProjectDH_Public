using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public virtual void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        UIManager.Instance.CloseUI(gameObject);
        // UIManager.Instance.Close(uiType);
    }

    public virtual void ClosePopupDelayUI(float delayTime)
    {
        UIManager.Instance.ClosePopupDelayUI(this, delayTime);
    }
}
