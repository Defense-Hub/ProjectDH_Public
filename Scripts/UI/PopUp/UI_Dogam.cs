using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dogam : UI_Popup
{
    enum Buttons
    {
        Btn_Common = 0,
        Btn_Rare = 1,
        Btn_Unique = 2,
        Btn_Legendary = 3,
        Btn_Epic = 4,
        Btn_Quit,
        Btn_All,
    }

    enum Objects
    {
        Obj_Commons = 0,
        Obj_Rares = 1,
        Obj_Uniques = 2,
        Obj_Legendarys = 3,
        Obj_Epics = 4,
        Obj_SelectAll = 5,
        Obj_SelectCommon = 6,
        Obj_SelectRare = 7,
        Obj_SelectUnique = 8,
        Obj_SelectLegendary = 9,
        Obj_SelectEpic =10,
    }

    [Serializable]
    class SlotInfo
    {
        public int Key;
        public UI_DogamSlot UI_DogamSlot;
    }

    //불, 얼음, 땅, 어둠  물, 순서 - 데이터에 저장된 key값으로 구분
    [SerializeField] private Color[] typeColor = new Color[5];
    // 커먼, 레어, 유니크, 레전더리, 에픽 순서
    [SerializeField] private Transform[] dogamTransform = new Transform[5];
    [SerializeField] private SlotInfo[] slots = new SlotInfo[25];

    private GameObject curSelectedBtn;
    private Dictionary<int, UI_DogamSlot> slotDicts = new Dictionary<int, UI_DogamSlot>();

    private void Start()
    {
        UIManager.Instance.UI_StartScene.UI_Dogam = this;
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Objects));
        Bind<Button>(typeof(Buttons));

        GetObject((int)Objects.Obj_SelectCommon).SetActive(false);
        GetObject((int)Objects.Obj_SelectRare).SetActive(false);
        GetObject((int)Objects.Obj_SelectUnique).SetActive(false);
        GetObject((int)Objects.Obj_SelectLegendary).SetActive(false);
        GetObject((int)Objects.Obj_SelectEpic).SetActive(false);

        GetButton((int)Buttons.Btn_Quit).AddOnClickEvent(ClosePopupUI);
        GetButton((int)Buttons.Btn_All).AddOnClickEvent(ActivateAll);
        GetButton((int)Buttons.Btn_Common).AddOnClickEvent(() => ActivateType(EUnitRank.Common));
        GetButton((int)Buttons.Btn_Rare).AddOnClickEvent(() => ActivateType(EUnitRank.Rare));
        GetButton((int)Buttons.Btn_Unique).AddOnClickEvent(() => ActivateType(EUnitRank.Unique));
        GetButton((int)Buttons.Btn_Legendary).AddOnClickEvent(() => ActivateType(EUnitRank.Legendary));
        GetButton((int)Buttons.Btn_Epic).AddOnClickEvent(() => ActivateType(EUnitRank.Epic));

        curSelectedBtn = GetObject((int)Objects.Obj_SelectAll);
        InitDogam();
    }

    private void InitDogam()
    {
        // 딕셔너리 초기화
        foreach (var slot in slots)
        {
            slotDicts[slot.Key] = slot.UI_DogamSlot;
        }

        // slot 초기화
        foreach(KeyValuePair<int, UnitBase> unitInfo in GameDataManager.Instance.UnitBases)
        {
            int key = unitInfo.Key;
            UnitBase val = unitInfo.Value;

            if(slotDicts.ContainsKey(key))
                slotDicts[key].SetDogamSlot(dogamTransform[key / 100], val.Thumbnail, typeColor[key%100], val.Name);
        }
    }

    // 활성화 된 버튼 바꾸기
    private void ActivateSelectBtn(GameObject nowSelectedBtn)
    {
        if (curSelectedBtn == nowSelectedBtn)
            return;

        curSelectedBtn.gameObject.SetActive(false);
        nowSelectedBtn.gameObject.SetActive(true);
        curSelectedBtn = nowSelectedBtn;
    }

    // 모든 등급 Activate
    private void ActivateAll()
    {
        // All 버튼 선택 활성화
        ActivateSelectBtn(GetObject((int)Objects.Obj_SelectAll));

        // 모든 도감 속성 활성화
        SetObjStatus(true);
    }

    // 특정 등급만 Activate
    private void ActivateType(EUnitRank eUnitRank)
    {
        // 활성화 해야 할 랭크 enum index
       int index = (int)eUnitRank;

        // 선택 활성화
        ActivateSelectBtn(GetObject(index + 6));

        // 모든 도감 속성 비활성화
        SetObjStatus(false);

        // 특정 도감 속성만 Activate
        GetObject(index).SetActive(true);
    }

    // 도감 속성 SetActive 속성 제어 메서드
    private void SetObjStatus(bool b)
    {
        GetObject((int)Objects.Obj_Commons).SetActive(b);
        GetObject((int)Objects.Obj_Rares).SetActive(b);
        GetObject((int)Objects.Obj_Uniques).SetActive(b);
        GetObject((int)Objects.Obj_Legendarys).SetActive(b);
        GetObject((int)Objects.Obj_Epics).SetActive(b);
    }
}
