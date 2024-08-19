using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButton : MonoBehaviour
{
    [SerializeField] private GameObject combinationBtn;
    [SerializeField] private GameObject sellBtn;
    [SerializeField]  private GameObject unitStatusUI;
    
    public async void Init()
    {
        unitStatusUI = await ResourceManager.Instance.GetUIGameObject(EUIRCode.UI_UnitStatus);
        unitStatusUI.SetActive(false);
    }

    public void DisableCombinationUI()
    {
        if (unitStatusUI == null)
        {
            unitStatusUI = GameManager.Instance.UnitSpawn.Controller.UnitTiles[0].UnitStatus.gameObject;
        }
        
        combinationBtn.SetActive(false);
        sellBtn.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.UnitSpawn.Controller.UnitAttackRange.gameObject.SetActive(false);
        unitStatusUI.SetActive(false);
    }
}
