using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public UI_StartScene UI_StartScene { get; set; }
    public UI_Interface UI_Interface { get; set; }
    public UI_HpBarCanvas UI_EnemyHpBar { get; set; }
    public Transform WorldCanvasTransform{get; set;}

    int _order = 10;


    
    UI_HUD _sceneUI = null;

    private Coroutine delayCoroutine;
    
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    public void CloseUI(GameObject ui)
    {
        ui.gameObject.SetActive(false);
        _order--;
    }
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // canvas.overrideSorting = true;
        //
        // if (sort)
        // {
        //     canvas.sortingOrder = _order;
        //     _order++;
        // }
        // else
        // {
        //     canvas.sortingOrder = 0;
        // }
    }
    
    public async Task<T> ShowPopupUI<T>(EUIRCode uiType , string name = null ) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        
        GameObject go = await ResourceManager.Instance.GetUIGameObject(uiType);
        T popup = Util.GetOrAddComponent<T>(go);
        
        go.transform.SetParent(Root.transform);

        return popup;
    }
    
    public void ClosePopupDelayUI(UI_Popup popup, float delayTime)
    {
        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }
        
        delayCoroutine = StartCoroutine(ClosePopupAfterDelay(popup, delayTime));
    }

    private IEnumerator ClosePopupAfterDelay(UI_Popup popup, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        popup.gameObject.SetActive(false);
    }
    
    public void DamageUI(float damage, Transform transform)
    {
        UI_DamageTXT damageTXT = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_DamageText).ReturnMyComponent<UI_DamageTXT>();
        damageTXT.SetDamageTXT(damage, transform);

    }
}
