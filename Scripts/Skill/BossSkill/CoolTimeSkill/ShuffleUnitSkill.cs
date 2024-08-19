using System.Collections;
using UnityEngine;

public class ShuffleUnitSkill : CoolTimeSkill
{
    [Header("# Shuffle Unit Skill Effect")]
    [SerializeField] private GameObject shuffleBossEffect;
    [SerializeField] private float shuffleUnitEffectDuration;

    private WaitForSeconds shuffleUnitEffectWaitForSeconds;
    private Coroutine shuffleUnitCoroutine;
    private Coroutine shuffleUnitEffectCoroutine;

    private void Start()
    {
        shuffleUnitEffectWaitForSeconds = new WaitForSeconds(shuffleUnitEffectDuration);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (IsReadyToUse() && !isStun)
        {
            isReadyToUseSkill = false;
            boss.SkillHandler.SetSkillInfo(
                ESkillType.CoolTime, 
                0.4f, 
                boss.AnimationData.Skill1ParameterHash, 
                () => {
                    if (shuffleUnitCoroutine != null)
                        StopCoroutine(shuffleUnitCoroutine);
                    if (shuffleUnitEffectCoroutine != null)
                        StopCoroutine(shuffleUnitEffectCoroutine);

                    shuffleUnitCoroutine = StartCoroutine(ShuffleUnitSKill());
                    shuffleUnitEffectCoroutine = StartCoroutine(ShuffleUnitSkillEffect()); }
                );
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (shuffleUnitCoroutine != null)
            StopCoroutine(shuffleUnitCoroutine);
        if (shuffleUnitEffectCoroutine != null)
            StopCoroutine(shuffleUnitEffectCoroutine);

        // 스킬 시전 중 죽었을 때 예외처리
        shuffleBossEffect.SetActive(false);
        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
    }

    private IEnumerator ShuffleUnitSKill()
    {
        UIManager.Instance.UI_Interface.ActivateBossSkillIndicator($"보스가 유닛 위치를 뒤섞습니다.");

        GameManager.Instance.CameraEvent.CameraShakeEffect(4f);
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.ShuffleUnitSkill);
        GameManager.Instance.UnitSpawn.Controller.TileEvents.ShuffleUnitTiles();
        yield return moveDelayWaitForSeconds;

        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.SkillHandler.FinishSkillCast();
        StartCoroutine(SkillCoolTime());
    }

    private IEnumerator ShuffleUnitSkillEffect()
    {
        shuffleBossEffect.SetActive(true);
        yield return shuffleUnitEffectWaitForSeconds;
        shuffleBossEffect.SetActive(false);
    }
}
