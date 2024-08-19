using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sell : MonoBehaviour
{
    [SerializeField] private Vector3 distance;
    [SerializeField] private UI_Combination uiCombination;
    [SerializeField] private Image goldIcon;
    [SerializeField] private Image gemStoneIcon;
    [SerializeField] private GameObject blockImage;
    [SerializeField] private TextMeshProUGUI sellTXT;


    [SerializeField] private UnitTile tile;
    private RectTransform rect;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnSellBtn(EUnitRank rank, UnitTile tile)
    {
        SoundManager.Instance.PlayUISfx(EUISfx.OnBtnClick);
        //To-do: Data List/Dictionary에서 받아오기
        this.tile = tile;

        if (rank >= EUnitRank.Epic)
        {
            return;
        }

        sellTXT.text = ($"+{GameManager.Instance.System.Sell.GetSellCurrency(rank)}");

        gameObject.SetActive(true);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(tile.transform.position);
        rect.position = screenPosition + distance;

        if (rank < EUnitRank.Rare)
        {
            goldIcon.gameObject.SetActive(true);
            gemStoneIcon.gameObject.SetActive(false);
        }
        else
        {
            gemStoneIcon.gameObject.SetActive(true);
            goldIcon.gameObject.SetActive(false);
        }
    }

    //전역변수로 타일 저장
    public void SellUnit()
    {
        if (tile == null)
        {
            Debug.LogError("Tile Null");
            return;
        }

        GameManager.Instance.System.Sell.SellUnit(tile.TileNum);
        DisableCombinationUI();
    }

    public void DisableCombinationUI()
    {
        if (tile != null)
        {
            if (tile.UnitStatus != null)
            {
                tile.UnitStatus.gameObject.SetActive(false);
            }

            tile = null;
        }

        gameObject.SetActive(false);
        uiCombination.gameObject.SetActive(false);
        blockImage.gameObject.SetActive(false);
        GameManager.Instance.UnitSpawn.Controller.UnitAttackRange.gameObject.SetActive(false);
    }
}