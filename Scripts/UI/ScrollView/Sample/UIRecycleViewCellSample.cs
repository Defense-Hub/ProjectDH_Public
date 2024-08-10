using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICellSampleData
    {
        [Header("Mission 1")]
        public int index;
        public string name;
    }

    public class UIRecycleViewCellSample : UIRecycleViewCell<UICellSampleData>
    {
        [SerializeField] private TextMeshProUGUI nIndex;
        [SerializeField] private TextMeshProUGUI txtName;

        public override void UpdateContent(int key, List<MaterialInfo> materialInfos)
        {
            Debug.Log("지승도 화이팅!");
        }

        /*        public override void UpdateContent(UICellSampleData itemData)
                {
                    nIndex.text = itemData.index.ToString();
                    txtName.text = itemData.name;
                }

                public void onClickedButton()
                {
                    Debug.Log(nIndex.text.ToString());
                }*/
    }
}
