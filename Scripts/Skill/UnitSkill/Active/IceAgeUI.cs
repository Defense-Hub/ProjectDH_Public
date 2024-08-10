using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IceAgeUI : MonoBehaviour
{
    [SerializeField] private Button activeBtn;
    [SerializeField] private Image attackRange;
    private float fillTime;
    private float fillAmountRate;
    private Coroutine fillCoroutine;
    
    public void Init(IceAge activeSkill)
    {
        activeBtn.onClick.RemoveAllListeners();
        activeBtn.onClick.AddListener(() => activeSkill.ActiveSkill());
        fillTime = activeSkill.FillTime;
        attackRange.transform.localScale = Vector3.zero;

        if(fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillRange(activeSkill));
    }

    private IEnumerator FillRange(IceAge activeSkill)
    {
        float count = 0;
        while (count < fillTime)
        {
            fillAmountRate = count / fillTime;
            activeSkill.DamageRate = fillAmountRate;
            activeBtn.image.fillAmount = fillAmountRate;
            attackRange.fillAmount = fillAmountRate;
            attackRange.transform.localScale = Vector3.one * fillAmountRate;
            count += Time.deltaTime;
            yield return null;
        }
        fillAmountRate = 1;
        activeSkill.DamageRate = fillAmountRate;
        activeSkill.ActiveSkill();
    }

    public void Disable()
    {
        StopCoroutine(fillCoroutine);
        Destroy(gameObject);
    }
}
