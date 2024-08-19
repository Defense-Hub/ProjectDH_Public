using System.Collections;
using UnityEngine;

public class BlockUnitSpawnSkill : CoolTimeSkill
{
    [Header("# Block Unit Spawn Skill")]
    [SerializeField] private int blockDuration;
    [SerializeField] private GameObject uiPrefab;

    [Header("# Block Unit Spawn Effect")]
    [SerializeField] private GameObject blockEffect;

    private Coroutine bossToUseSkillCoroutine;
    private Boss_Water waterBoss;

    private void Start()
    {
        waterBoss = boss as Boss_Water;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (IsReadyToUse() && !isStun && UIManager.Instance.UI_Interface.CanSummon())
        {
            isReadyToUseSkill = false;
            boss.SkillHandler.SetSkillInfo(
                ESkillType.CoolTime,
                0.7f, 
                boss.AnimationData.Skill2ParameterHash, 
                () => {
                    BlockUnitSpawn();

                    if (bossToUseSkillCoroutine != null)
                        StopCoroutine(bossToUseSkillCoroutine);
                    bossToUseSkillCoroutine= StartCoroutine(BlockUnitSkillEffect());}
                );
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (bossToUseSkillCoroutine != null)
            StopCoroutine(bossToUseSkillCoroutine);
        blockEffect.SetActive(false);

        if (GameManager.Instance.Stage.SpawnEnemyList.Count == 0 && waterBoss.CurSplitCount ==0)
        {
            UIManager.Instance.UI_Interface.UnBlockSummonBtn(false);
            UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
            if (waterBoss.ui_CountDown != null)
                waterBoss.ui_CountDown.DestroyUI();
        }
    }

    private void BlockUnitSpawn()
    {
        if (waterBoss.ui_CountDown != null)
        {
            UIManager.Instance.UI_Interface.ActivateBossSkillIndicator($"보스가 유닛 소환 버튼을 막습니다");
            UIManager.Instance.UI_Interface.UnBlockSummonBtn(true);
            SoundManager.Instance.PlayInGameSfx(EInGameSfx.BlockUnitSpawnSkill);

            waterBoss.ui_CountDown.StartCountDown(
                blockDuration,
                () => {
                    UIManager.Instance.UI_Interface.UnBlockSummonBtn(false);
                    UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
                });
        }
    }

    private IEnumerator BlockUnitSkillEffect()
    {
        blockEffect.SetActive(true);
        GameManager.Instance.CameraEvent.CameraShakeEffect(1.5f);
        yield return moveDelayWaitForSeconds;
        blockEffect.SetActive(false);
        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.SkillHandler.FinishSkillCast();
        StartCoroutine(SkillCoolTime());
    }
}
