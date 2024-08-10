using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TeleportSkill : CoolTimeSkill
{
    [Header("# Teleport Skill")]
    [SerializeField] private int teleportCount;
    [SerializeField] private float teleportTerm;

    [Header("# Teleport Skill Effect")]
    [SerializeField] private GameObject teleportSkillEffect;
    [SerializeField] private float teleportEffectDuration;
    [SerializeField] private TextMeshProUGUI teleportTXT;

    private WaitForSeconds teleportDelay;
    private WaitForSeconds teleportEffectWaitForSeconds;
    private Coroutine teleportCoroutine;

    private int prevIndex;
    private int curTeleportCount;

    private void Start()
    {
        teleportDelay = new WaitForSeconds(teleportTerm);
        teleportEffectWaitForSeconds = new WaitForSeconds(teleportEffectDuration);
        prevIndex = boss.TargetIndex-1 < 0 ? 3: boss.TargetIndex - 1;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (IsReadyToUse() && !isStun)
        {
            isReadyToUseSkill = false;
            boss.SkillHandler.SetSkillInfo(
                ESkillType.CoolTime, 
                0.7f, 
                boss.AnimationData.Skill2ParameterHash, 
                () => {
                    if(teleportCoroutine != null)
                        StopCoroutine(teleportCoroutine);

                    teleportCoroutine = StartCoroutine(Teleport());  }
                );
            curTeleportCount = teleportCount;
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (teleportCoroutine != null)
            StopCoroutine(teleportCoroutine);

        // 스킬 시전 중 죽었을 때 예외처리
        teleportSkillEffect.SetActive(false);
        DisableTXT(teleportTXT);
    }

    public Vector3 GetRandomWayPoint()
    {
        int randomIndex;
        // 이전에 텔레포트한 라인으로는 텔레포트 하지 않도록 처리
        while (true)
        {
            // 랜덤 웨이포인트 Index 뽑기
            randomIndex = Random.Range(0, boss.WayPoints.Length);
            if (randomIndex != prevIndex && randomIndex !=boss.TargetIndex-1)
            {
                prevIndex = randomIndex;
                boss.SetTargetIndex((randomIndex + 1) % boss.WayPoints.Length);
                break;
            }
        }

        Vector3 wp1 = boss.WayPoints[randomIndex].position;
        Vector3 wp2 = boss.WayPoints[(randomIndex + 1) % boss.WayPoints.Length].position;

        // 두 웨이포인트 사이 거리 비율 지정 - 웨이포인트와 얼마나 가까운 지점을 뽑을지
        float randomValue = Random.Range(0.3f, 0.7f);
    
        return Vector3.Lerp(wp1, wp2, randomValue);
    }

    private IEnumerator Teleport()
    {
        if (curTeleportCount == 0) 
        {
            boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
            boss.SkillHandler.FinishSkillCast();
            StartCoroutine(SkillCoolTime());
            yield break;
        }
        // 보스 회전 -> 화면 보도록
        boss.transform.rotation = Quaternion.Euler(0, 180, 0);

        Vector3 teleportPos= GetRandomWayPoint();
        teleportSkillEffect.SetActive(true);
        teleportSkillEffect.transform.position = teleportPos;
        GameManager.Instance.CameraEvent.CameraShakeEffect(0.3f);
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.TeleportSkill);

        yield return teleportEffectWaitForSeconds;
        teleportSkillEffect.SetActive(false);
        boss.transform.position = teleportPos;
        InitTXT(teleportTXT, transform.position + Vector3.up * 5f, 0.2f, "텔레포트!", teleportTerm);
        curTeleportCount--;

        yield return teleportDelay;
        DisableTXT(teleportTXT);
        boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
    }
}
