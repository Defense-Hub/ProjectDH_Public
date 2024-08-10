using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIRecycleViewCellMyth : UIRecycleViewCell<CombinationData>
{
    [SerializeField] private Image character;
    [SerializeField] private TextMeshProUGUI prograss;
    
    private Button epicButton;
    private int targetID;
    private MythBox mythBox;
    
    public void Init(int key, MythBox mythBox)
    {
        targetID = key;
        this.mythBox = mythBox;

        character.sprite = GameDataManager.Instance.UnitBases[key].Thumbnail;
        
        epicButton = GetComponent<Button>();
        
        epicButton.onClick.AddListener(EpicButtonOnClickEvent);
    }
    

    public void SetProbablilty()
    {
        List<MaterialInfo> materialInfos = GameManager.Instance.System.Combination.AdvancedCombinationDict[targetID];

        float total = 100;
        float cur = 0;
        foreach (var materialInfo in materialInfos)
        {
            if (materialInfo.hasUnit)
            {
                float value = GameDataManager.Instance.UnitBases[materialInfo.targetID].UnitRank switch
                {
                    EUnitRank.Legendary => 50f,
                    EUnitRank.Unique => 35f,
                    EUnitRank.Rare => 30f,
                    EUnitRank.Common => 20f,
                };

                cur += value;
            }
        }

        cur = Mathf.Min(cur, 100f);

        int per = Mathf.FloorToInt((cur / total) * 100);
        prograss.text = $"진행률 {per}%";
    }
    private void EpicButtonOnClickEvent()
    {
        UpdateContent(targetID, GameManager.Instance.System.Combination.AdvancedCombinationDict[targetID]);
    }
    
    public override void UpdateContent(int key, List<MaterialInfo> materialInfos)
    {
        mythBox.SetEpicCombinationUI(key, materialInfos);
    }
}
