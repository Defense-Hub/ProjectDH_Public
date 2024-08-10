using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Myth : UI_Popup
{
    public MythBox MythBox { get; set; }
    //enum
    enum Buttons
    {
        Btn_Close,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Btn_Close).AddOnClickEvent(CloseUIOnClickEvent);    
    }
    
    public void CloseUIOnClickEvent()
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);
        MythBox.ResetMythBox();
        ClosePopupUI();
    }
    

/*    public void SetInfo(int id)
    {
        Debug.Log(id);
        int target = GameDataManager.Instance.CombinationDatas[id].targetID;

        CombinationData combinationData = GameDataManager.Instance.CombinationDatas[target];



        int Cnt = GameDataManager.Instance.CombinationDatas[id].materialIDs.Count;
        [SerializeField] private TextMeshProUGUI name = GameDataManager.Instance.UnitBases[target].Name;

        Debug.Log(Cnt);
        Debug.Log(name);
    }*/
}