using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButton : MonoBehaviour
{
    [SerializeField] private GameObject combinationBtn;
    [SerializeField] private GameObject sellBtn;

    void OnEnable()
    {
        CheckActive(combinationBtn);
    }

    private void CheckActive(GameObject btn)
    {
/*        if (!combinationBtn.gameObject.activeSelf)
        {
            Debug.Log("Disable Image");
            gameObject.SetActive(false);
        }*/
    }

    public void DisableCombinationUI()
    {
        /*        tile = null;*/
        combinationBtn.SetActive(false);
        sellBtn.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.UnitSpawn.Controller.UnitAttackRange.gameObject.SetActive(false);
        /*CheckActive(combinationBtn);*/
    }
}
