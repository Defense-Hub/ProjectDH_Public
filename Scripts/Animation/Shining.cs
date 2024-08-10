using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Shining : MonoBehaviour
{
    public Image targetImage; // 애니메이션을 적용할 이미지
    public float duration = 1f; // 애니메이션 지속 시간
    public float scaleMultiplier = 1.5f; // 이미지가 커지는 정도

    void Start()
    {
        // 이미지의 초기 크기
        Vector3 originalScale = targetImage.rectTransform.localScale;

        // 이미지를 커졌다 작아지는 애니메이션 설정
        targetImage.rectTransform.DOScale(originalScale * scaleMultiplier, duration)
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복, Yoyo 타입(커졌다 작아짐)
            .SetEase(Ease.InOutSine); // 부드러운 애니메이션을 위해 Ease 설정
    }
}
