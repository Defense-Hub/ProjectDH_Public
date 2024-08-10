using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIRecycleViewControllerMyth : UIRecycleViewSystemMyth<CombinationData>
    {
        private bool isCellLoad;
        // 리스트 항목의 데이터를 읽어 들이는 메서드
        private void LoadData()
        {
            //미션 데이터 가져옴                                    

            InitializeTableView();

            isCellLoad = true;
            //조합창 초기화
/*            UpdateCellForIndex(cell, index);*/
        }

        // Called when the controller is initialized
        protected override void Awake()
        {
            base.Awake();
            
        }

        protected void OnEnable()
        {
            if (isCellLoad)
            {
                SetEpicCombinationProbability();
            }
            else
            {
                LoadData();
            }
        }

        // Method called when a cell is pressed
        public void OnPressCell(UIRecycleView<CombinationData> cell)
        {
            Debug.Log("Cell Clicked");
        }
    }
}
