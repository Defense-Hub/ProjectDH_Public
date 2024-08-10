using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythButton : MonoBehaviour
{
    /*public UI_Myth myth;*/
    private UIRecycleViewCellMyth myth;
    private int id;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickMythBtn);

    }

/*    private void OnClickMythBtn()
    {
        myth.SetInfo(id);
    }*/

    public void OnClickMythBtn()
    {
        /*int key, List< MaterialInfo > materialInfos*/


/*        for (int i = 0; i < GameManager.Instance.System.Combination.AdvancedCombinationDict.Count; i++)
        {*/
            myth.UpdateContent(id, GameManager.Instance.System.Combination.AdvancedCombinationDict[id+400]);
        

        //key값을 targetID로
        //진행률순으로 정렬
    }
}
