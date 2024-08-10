using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIRecycleViewControllerSample : UIRecycleViewController<UICellSampleData>
    {
        //리스트 항목의 데이터를 읽어 들이는 메서드
        private void LoadData()
        {
            //일반적인 데이터는 데이터 소스로부터 가져오는 데 여기서는 하드 코드를 사용하여 정의
            /*tableData = new List<UICellSampleData>()
            {
                new UICellSampleData {index = 1, name = "One"},
                new UICellSampleData {index = 2, name = "Two"},
                new UICellSampleData {index = 3, name = "Three"},
                new UICellSampleData {index = 4, name = "Four"}
            };*/

            //스크롤시킬 내용의 크기 갱신
            InitializeTableView();
        }

        //인스턴스를 로드할 때 Awake 메서드가 처리된 다음에 호출
/*        protected override void Start()
        {
            // 기반 클래스의 Start 메서드를 호출
            base.Start();

            // 리스트 항목의 데이터를 읽어들임
            LoadData();
        }*/

        // 셀이 선택됐을 때 호출되는 메서드
        /*public void OnPressCell(UIRecycleViewCellSample cell)
        {
            Debug.Log("Cell Clicked");
            Debug.Log(tableData[cell.Index].name);
        }*/
    }
}
