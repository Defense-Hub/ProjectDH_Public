using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UI_DamageTXT : PoolObject
{
    [SerializeField] private TMP_Text damageTXT;
    [SerializeField] private RectTransform rt;
    private Camera mainCamera;

    Sequence sequence;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetDamageTXT(float damage, Transform targetTransform)
    {
        InitializeDamageTXT();

        damageTXT.text = damage.ToString();

        // 위치 설정
        Vector3 offsetPosition = targetTransform.position + Vector3.up*0.85f;
        transform.position = offsetPosition;
        transform.localScale = Vector3.one;



        //애니매이션
        sequence = DOTween.Sequence();
        sequence.Append(rt.DOMoveY(rt.position.y + 0.5f, 0.5f).SetEase(Ease.OutQuad));
        sequence.Join(damageTXT.DOFade(0, 0.3f).SetEase(Ease.OutQuad).SetDelay(0.2f));
        sequence.Insert(0, DOTween.To(() => damageTXT.color, x => damageTXT.color = x, GetDamageColor(damage), 0));
        sequence.OnComplete(() => { gameObject.SetActive(false); });

        sequence.Restart();

        // 카메라를 바라보도록 설정
        LookAtCamera();

    }

    private void LookAtCamera()
    {
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }

    private void InitializeDamageTXT()
    {
        // 불투명도 초기화
        Color color = damageTXT.color;
        color.a = 1.0f; // 알파 값을 1로 설정하여 불투명도로 초기화
        damageTXT.color = color;
    }

    private Color GetDamageColor(float damage)
    {
        if (damage <= 24)
        {
            return Color.white;
        }
        else if (damage >= 25 && damage < 27)
        {
            return Color.yellow;
        }
        else if (damage >= 27)
        {
            return Color.red;
        }
        return Color.white;
    }

}
