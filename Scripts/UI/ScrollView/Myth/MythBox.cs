using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MythBox : MonoBehaviour
{
    [Header("# Epic Combination Info")]
    [SerializeField] private TextMeshProUGUI unitTitle;
    [SerializeField] private TextMeshProUGUI targetUnitText;
    [SerializeField] private Image targetUnitThumbnail;
    [SerializeField] private Button combinationBtn;
    [SerializeField] private Image readyImage;
    
    [Header("# Material Info")]
    [SerializeField] private CombinationMaterialUI[] combinationMaterialUIs;

    [SerializeField]private GameObject boxUI;
    private bool isCombinationOn = true;
    private int curTargetID;
    private UI_Myth mythUI;
    private bool isClose;
    private void Awake()
    {
        mythUI = transform.root.GetComponent<UI_Myth>();
        mythUI.MythBox = this;
        
        boxUI = transform.GetChild(0).gameObject;
        ResetMythBox();
        curTargetID = -1;
        isClose = true;
    }
    
    public void SetEpicCombinationUI(int targetID, List<MaterialInfo> materialInfos)
    {
        if(!isClose && curTargetID == targetID)
            return;

        curTargetID = targetID;
        
        combinationBtn.onClick.RemoveAllListeners();
        isClose = false;
        isCombinationOn = true;
            
        for (int i = 0; i < materialInfos.Count; i++)
        {
            if(i >= combinationMaterialUIs.Length)
                break;
            
            combinationMaterialUIs[i].character.sprite = GameDataManager.Instance.UnitBases[materialInfos[i].targetID].Thumbnail;
            combinationMaterialUIs[i].checkIcon.gameObject.SetActive(materialInfos[i].hasUnit);
            combinationMaterialUIs[i].hasUnitText.text = materialInfos[i].hasUnit ? "보유" : "미보유";

            combinationMaterialUIs[i].gameObject.SetActive(true);
            
            if (!materialInfos[i].hasUnit)
            {
                isCombinationOn = false;
            }
        }

        UnitBase targetUnit = GameDataManager.Instance.UnitBases[targetID];

        unitTitle.text = targetUnitText.text = targetUnit.Name;
        targetUnitThumbnail.sprite = targetUnit.Thumbnail;
        
        combinationBtn.interactable = isCombinationOn;
        readyImage.gameObject.SetActive(!isCombinationOn);

        if (isCombinationOn)
        {
            combinationBtn.AddOnClickEvent(TryEpicCombination);
        }
        
        boxUI.SetActive(true);
    }

    public void ResetMythBox()
    {
        isClose = true;
        boxUI.SetActive(false);
    }

    private void TryEpicCombination()
    {
        mythUI.CloseUIOnClickEvent();
        GameManager.Instance.System.TryCombinationAdvancedUnit(curTargetID);
    }
    
}
