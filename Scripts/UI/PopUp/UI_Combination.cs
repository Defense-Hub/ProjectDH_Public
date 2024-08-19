using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Combination : MonoBehaviour
{
    [SerializeField] private Vector2 distance;
    [SerializeField] private GameObject blockImage;
    [SerializeField] private UI_Sell uiSell;

    [SerializeField] private UnitTile tile;
    [field:SerializeField]public RectTransform Rect { get; private set; }


    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    public bool IsCombinationLock { get; set; }
    public void SetCombinationUI(UnitTile tile)
    {
        if(tile.UnitCount < 2 || IsCombinationLock)
            return;

        this.tile = tile;
        uiSell.gameObject.SetActive(true);
        gameObject.SetActive(true);
        //blockImage.SetActive(true);

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(tile.transform.position);
        Rect.position = screenPosition + distance;
    }
    
    public void CombinationUnit()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);
        GameManager.Instance.System.Combination.CombinationBasicUnit(tile);
        DisableCombinationUI();
    }

    public void DisableCombinationUI()
    {
        if (tile != null)
        {
            if (tile.UnitStatus != null)
            {
                tile.UnitStatus.gameObject.SetActive(false);
            }

            tile = null;
        }
        
        gameObject.SetActive(false);
        blockImage.SetActive(false);
        uiSell.gameObject.SetActive(false);
        GameManager.Instance.UnitSpawn.Controller.UnitAttackRange.gameObject.SetActive(false);
    }

    public void TutorialCombination()
    {
        GameManager.Instance.System.Combination.CombinationBasicUnit(tile);
        DisableCombinationUI();
        IsCombinationLock = true;
        GameManager.Instance.Tutorial.TutorialUIController.DialogueEvent.StartDialouge();
    }

    public void SetCombinationLock(bool isTrue)
    {
        IsCombinationLock = isTrue;
    }
}
