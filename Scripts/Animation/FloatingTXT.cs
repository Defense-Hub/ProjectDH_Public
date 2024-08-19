using UnityEngine;
using TMPro;  // TextMeshPro를 사용하기 위해 필요
using DG.Tweening;

public class FloatingTXT : MonoBehaviour
{
    [SerializeField] private GameObject coinTextPrefab;  // 돈 획득 텍스트 프리팹을 GameObject로 받음
    [SerializeField] private float floatDistance = 25f;  // 텍스트가 상승할 거리
    [SerializeField] private float duration = 1.5f;  // 텍스트가 유지될 시간

    public void ShowFloatingText(int amount)
    {
        // 프리팹을 인스턴스화
        GameObject coinTextObj = Instantiate(coinTextPrefab, coinTextPrefab.transform.position, coinTextPrefab.transform.rotation, transform);

        // TextMeshProUGUI 컴포넌트를 가져와서 텍스트 설정
        TextMeshProUGUI coinText = coinTextObj.GetComponent<TextMeshProUGUI>();
        if (coinText != null)
        {
            coinText.text = "+" + amount.ToString() + " Coins";
        }

        // 텍스트가 위로 떠오르는 애니메이션
        coinTextObj.transform.DOMoveY(coinTextObj.transform.position.y + floatDistance, duration).SetEase(Ease.OutQuad);

        // 텍스트가 서서히 사라지는 애니메이션
        if (coinText != null)
        {
            coinText.DOFade(0, duration).OnComplete(() =>
            {
                // 애니메이션 완료 후 텍스트 오브젝트 삭제
                Destroy(coinTextObj);
            });
        }
    }
}
