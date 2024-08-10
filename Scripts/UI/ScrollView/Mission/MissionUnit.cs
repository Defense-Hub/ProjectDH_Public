using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MissionUnit : MonoBehaviour
{
    [SerializeField] private Image materialImages;
    [SerializeField] private Image checkIcon;
    [SerializeField] private TextMeshProUGUI getUnitTXT;
    [SerializeField] public int materialIDs;

    //버그: UI창 껐다 켰는데도, 활성화 안됨. 드래그 해야 업데이트 됨
    
    // 조회: 유닛이 들어와 있는지 체크하는 것
    // Asset번들에서 어떻게 가져와요??   
    // ID 통해서 이미지 가져오기
    // 해당되는 몬스터 생성됐는지 조회하는 로직
    // 캐릭터 가져와야함...

    public void MissionUnitSetting(int ID, bool hasUnit)
    {
        gameObject.SetActive(true);
        materialImages.sprite = GameDataManager.Instance.UnitBases[ID].Thumbnail;

        //체크        ss
        checkIcon.gameObject.SetActive(hasUnit);
        getUnitTXT.text = hasUnit ? "보유" : "미보유";
    }

}
