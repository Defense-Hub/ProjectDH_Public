using System.Collections;
using UnityEngine;

public class InvincibleSkill : CoolTimeSkill
{
    [Header("# Invincible Skill")]
    [SerializeField] private int invincibleDuration;

    [Header("# Invincible Skill Effect")]
    [SerializeField] private GameObject invincibleEffect;
    [SerializeField] private GameObject countDownUIPrefab;

    private UI_CountDown ui_CountDown;
    private WaitForSeconds invincibleWaitForSeconds;
    private Coroutine invincivleCoroutine;

    private void Start()
    {
        invincibleWaitForSeconds = new WaitForSeconds(invincibleDuration);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        InitCountDownUI();
        if (IsReadyToUse() && !isStun)
        {
            isReadyToUseSkill = false; boss.SkillHandler.SetSkillInfo( 
                ESkillType.CoolTime, 
                1f,
                boss.AnimationData.IdleParameterHash,  
                () =>
                {
                    if (invincivleCoroutine != null)
                        StopCoroutine(invincivleCoroutine);
                    invincivleCoroutine = StartCoroutine(Invincible());
                });
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (invincivleCoroutine != null)
            StopCoroutine(invincivleCoroutine);

        invincibleEffect.SetActive(false);
        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();

        if (boss!= null)
            boss.HealthSystem.ChangeInvincibility(false);
        if(ui_CountDown!= null)
            ui_CountDown.DestroyUI();
    }

    private IEnumerator Invincible()
    {
        UIManager.Instance.UI_Interface.ActivateBossSkillIndicator($"보스가 일정 시간 무적이 됩니다");
        ui_CountDown.StartCountDown(invincibleDuration, () => UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator());

        SoundManager.Instance.PlayInGameSfx(EInGameSfx.InvincibleSkill);
        boss.HealthSystem.ChangeInvincibility(true);
        invincibleEffect.SetActive(true);
        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);

        yield return invincibleWaitForSeconds;

        boss.HealthSystem.ChangeInvincibility(false);
        invincibleEffect.SetActive(false);
        
        boss.SkillHandler.FinishSkillCast();
        StartCoroutine(SkillCoolTime());
    }

    private void InitCountDownUI()
    {
        if (ui_CountDown == null)
        {
            GameObject prefab = Instantiate(countDownUIPrefab);
            ui_CountDown = prefab.GetComponent<UI_CountDown>();
            ui_CountDown.InitUI();
        }
    }
}
