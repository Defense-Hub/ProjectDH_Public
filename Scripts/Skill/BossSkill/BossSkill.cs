using DG.Tweening;
using TMPro;
using UnityEngine;

public class BossSkill : Skill
{
    protected Boss boss;
    public void Init(Boss boss)
    {
        this.boss = boss;
    }

    protected void InitTXT(TextMeshProUGUI txt, Vector3 txtPos, float dotTargetYPos, string text, float duration)
    {
        if (txt != null)
        {
            txt.gameObject.SetActive(true);
            txt.transform.SetParent(UIManager.Instance.WorldCanvasTransform);
            txt.transform.position = txtPos;
            txt.transform.rotation = Quaternion.Euler(60, 0, 0);
            txt.text = text;

            // 닷트윈 
            float targetY = txt.transform.position.y + dotTargetYPos;
            txt.transform.DOMoveY(targetY, duration).SetEase(Ease.OutQuad);
        }
    }

    protected void DisableTXT(TextMeshProUGUI txt)
    {
        if (txt != null)
        {
            txt.gameObject.SetActive(false);
            txt.transform.SetParent(transform);
        }
    }
}
