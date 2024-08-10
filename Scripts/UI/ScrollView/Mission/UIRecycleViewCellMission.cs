using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //public List<Sprite> images; // isClear가 true 일때 활성화
    // 이미지 리스트는 'isClear'가 true일 때 활성화됩니다.

    public class UIRecycleViewCellMission : UIRecycleViewCell<MissionData>
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI goldRewardTXT;
        [SerializeField] private TextMeshProUGUI gemStoneRewardTXT;
        [SerializeField] private GameObject goldFrame;
        [SerializeField] private GameObject gemStoneFrame;
        [SerializeField] private Image ClearImage;          
        [SerializeField] private List<MissionUnit> materials;  // materialID에 대한 이미지 매핑

        private int missionIndex;
        private MissionSystem missionSystem;

        //스프라이트 리스트를 만들어두고
        //딕셔너리는 직렬화가 안된다
        //오딘 >> 유료      


        //개선사항: 미션 완료시, 더이상 반영안되게
        //완료가 안된 cell 먼저 그리기
        //완료가 될 시, if(렌더링x)
        //완료가 된 cell들만 렌더링

        public override void UpdateContent(int key, List<MaterialInfo> materialInfos)
        {
            MissionData missionData = GameDataManager.Instance.MissionDatas[key];
            MissionSystem missionSystem = GameManager.Instance.System.Mission;

            titleText.text = missionData.missionTitle;            

            goldRewardTXT.text = missionData.goldReward.ToString();
            goldFrame.gameObject.SetActive(missionData.goldReward>0);

            gemStoneRewardTXT.text = missionData.gemStoneReward.ToString();
            gemStoneFrame.gameObject.SetActive(missionData.gemStoneReward > 0);

            // materialID에 대응되는 이미지 활성화
            for (int i = 0; i < materialInfos.Count; i++)
            {
                //박스[idx] = 유닛ID
                materials[i].MissionUnitSetting(materialInfos[i].targetID, materialInfos[i].hasUnit);               
            }

            ClearImage.gameObject.SetActive(missionSystem.MissionClearArr[key]);
        }
    }
}
