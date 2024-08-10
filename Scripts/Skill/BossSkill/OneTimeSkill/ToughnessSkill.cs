using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ToughnessSkill : OneTimeSkill
{
    [Header("Toughness Skill")]
    [SerializeField][Range(0f, 100f)] private float toughnessAmount;

    [Header("Toughness Skill Effect")]
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private GameObject transparentGolem; 
    [SerializeField] private TextMeshProUGUI toughnessTXT;

    private Vector3 originScale;
    private Coroutine toughnessCoroutine;
    private Coroutine changeGolemScaleCoroutine;
    private float animDuration;

    private void Awake()
    {
        originScale = transparentGolem.transform.localScale;
    }

    public override void UseSkill()
    {
        boss.SkillHandler.SetSkillInfo(
            ESkillType.OneTime, 
            0.35f, 
            boss.AnimationData.Skill1ParameterHash,
            () => {
                if (toughnessCoroutine != null)
                    StopCoroutine(toughnessCoroutine);

                toughnessCoroutine = StartCoroutine(StartToughnessSkill()); }
            );
        boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (toughnessCoroutine != null)
            StopCoroutine(toughnessCoroutine);
        if (changeGolemScaleCoroutine != null)
            StopCoroutine(changeGolemScaleCoroutine);

        if (transparentGolem != null)
        {
            transparentGolem.transform.localScale = originScale;
            transparentGolem.SetActive(false);
        }
        DisableTXT(toughnessTXT);
    }

    private IEnumerator StartToughnessSkill()
    {
        boss.Stat.Toughness += toughnessAmount;
        animDuration = boss.Animator.GetCurrentAnimatorClipInfo(0).Length / boss.SkillHandler.SkillInfo.animSpeedMultiplier;
        InitTXT(toughnessTXT, transform.position + Vector3.up * 3f, 0.3f, $"강인함 + {toughnessAmount}!", animDuration);
        WaitForSeconds animSeconds = new WaitForSeconds(animDuration/2);
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.ToughnessSkill);

        changeGolemScaleCoroutine = StartCoroutine(ChangeTransparentGolemScale(animDuration/2, maxScale));
        yield return animSeconds;

        DisableTXT(toughnessTXT);
        changeGolemScaleCoroutine = StartCoroutine(ChangeTransparentGolemScale(animDuration/2, originScale));
        yield return animSeconds;

        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.SkillHandler.FinishSkillCast();
    }

    private IEnumerator ChangeTransparentGolemScale(float duration, Vector3 targetScale)
    {
        transparentGolem.SetActive(true);
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transparentGolem.transform.localScale = Vector3.Lerp(transparentGolem.transform.localScale, targetScale, t); 
            yield return null;
        }
        transparentGolem.SetActive(false);
    }
}