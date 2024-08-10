using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIRecycleViewControllerMission : UIRecycleViewController<MissionData>
    {
        // 리스트 항목의 데이터를 읽어 들이는 메서드
        private void LoadData()
        {
            //미션 데이터 가져옴                                    

            InitializeTableView();
        }

        // Called when the controller is initialized
        protected override void Awake()
        {
            base.Awake();
            
        }

        protected void OnEnable()
        {
            LoadData();
        }

        // Method called when a cell is pressed
        public void OnPressCell(UIRecycleViewCell<MissionData> cell)
        {
            Debug.Log("Cell Clicked");
            Debug.Log(GameDataManager.Instance.MissionDatas[cell.Index].missionTitle);
        }
    }
}
