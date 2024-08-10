using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorSkill : CoolTimeSkill
{
    [Header("# Meteor Skill")]
    [SerializeField][Range(0, 25)] private int meteorCount;
    [SerializeField] private float meteorTerm;
    [SerializeField] private float stunDuration;
    [SerializeField] private float meteorSpeed;

    private WaitForSeconds meteorTermWaitForSeconds;
    private readonly WaitForSeconds meteorActiveEffectWaitForSeconds = new WaitForSeconds(1f);
    private readonly List<Effect> activeEffects = new List<Effect>();
    private CCInfo meteorCCInfo;
    private Coroutine meteorCoroutine;
    private Coroutine deActiveEffectCoroutine;

    private void Start()
    {
        meteorTermWaitForSeconds = new WaitForSeconds(meteorTerm);
        SetMeteorCCInfo();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (IsReadyToUse() && !isStun)
        {
            isReadyToUseSkill = false;
            boss.SkillHandler.SetSkillInfo(
                ESkillType.CoolTime, 
                0.45f, 
                boss.AnimationData.Skill2ParameterHash, 
                ()=>
                {
                    if (meteorCoroutine != null)
                        StopCoroutine(meteorCoroutine);
                    meteorCoroutine = StartCoroutine(StartMeteorSkill());
                });
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (meteorCoroutine != null)
            StopCoroutine(meteorCoroutine);
        if (deActiveEffectCoroutine != null)
            StopCoroutine(deActiveEffectCoroutine);

        foreach (Effect effect in activeEffects)
        {
            if(effect != null)
                effect.gameObject.SetActive(false);
        }
        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
    }

    private IEnumerator StartMeteorSkill()
    {
        UIManager.Instance.UI_Interface.ActivateBossSkillIndicator($"보스가 메테오 스킬을 사용합니다.");
        foreach (UnitTile tile in CollectionUtils.GetUniqueCollections(GameManager.Instance.UnitSpawn.Controller.UnitTiles, meteorCount))
        {
            Meteor meteor = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_Meteo).ReturnMyComponent<Meteor>();
            meteor.transform.position = tile.transform.position + Vector3.up * 10f;

            Effect delayEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_MeteoBegin).ReturnMyComponent<Effect>();
            delayEffect.transform.position = tile.transform.position + Vector3.up * 0.1f;

            meteor.SetMeteorInfo(meteorSpeed, tile, delayEffect);
            meteor.OnMeteorHitUnit += CheckMeteorUnitCollision;

            yield return meteorTermWaitForSeconds;
        }
        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.SkillHandler.FinishSkillCast();
        StartCoroutine(SkillCoolTime());
    }

    private void CheckMeteorUnitCollision(UnitTile tile) 
    {
        // 해당 타일에 unit 존재하는지 체크
        bool isEmpty = tile.SpawnUnits.All(unit => unit == null);

        if (!isEmpty)
        {
            foreach (Unit unit in tile.SpawnUnits)
            {
                if (unit != null)
                {
                    if (unit.StateMachine.IsMoving()) return;
                    unit.StateMachine.ChangeHardCCState(meteorCCInfo);
                }
            }
            // 타일 자리에 폭발 Effect
            Effect activeEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_MeteoActive).ReturnMyComponent<Effect>();
            activeEffects.Add(activeEffect);
            activeEffect.transform.position = tile.transform.position + Vector3.up * 0.5f;
            SoundManager.Instance.PlayInGameSfx(EInGameSfx.MeteorSkill);

            if (deActiveEffectCoroutine != null)
                StopCoroutine(deActiveEffectCoroutine);
            if(activeEffect!=null && activeEffect.gameObject.activeSelf)
                deActiveEffectCoroutine = StartCoroutine(DeActiveMeteorEffect(activeEffect));
        }
    }

    private IEnumerator DeActiveMeteorEffect(Effect effect)
    {
        yield return meteorActiveEffectWaitForSeconds;
        activeEffects.Remove(effect);
        effect.gameObject.SetActive(false);
    }

    private void SetMeteorCCInfo()
    {
        meteorCCInfo = new CCInfo
        {
            ccType = ECCType.Stun,
            duration = stunDuration,
        };
    }
}