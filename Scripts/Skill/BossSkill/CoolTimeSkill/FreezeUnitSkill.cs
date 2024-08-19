using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeUnitSkill : CoolTimeSkill
{
    [Header("# Freeze Unit Skill")]
    [SerializeField][Range(0, 25)] private int freezeTileCount;
    [SerializeField] private float freezePreparationDuration;
    [SerializeField] private float freezeDuration;

    [Header("# Freeze Unit Skill")]
    [SerializeField] private GameObject freezeEffect;

    private List<UnitTile> canFreezeTiles = new List<UnitTile>();

    private TileStatusEffectInfo effectInfo;
    private Coroutine freezeUnitCoroutine;

    private WaitForSeconds wait;
    private Coroutine freezeTimerCoroutine;
    
    private void Start()
    {
        wait = new WaitForSeconds(coolTime - 1f);
        SetTileStatusEffectInfo();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (IsReadyToUse() && !isStun)
        {
            isReadyToUseSkill = false;
            boss.SkillHandler.SetSkillInfo(
                ESkillType.CoolTime, 
                0.3f, 
                boss.AnimationData.Skill2ParameterHash, 
                () =>{
                    if (freezeUnitCoroutine != null)
                        StopCoroutine(freezeUnitCoroutine);

                    freezeUnitCoroutine = StartCoroutine(FreezeUnit()); }
                );
            boss.StateMachine.ChangeState(boss.StateMachine.SkillState);
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        if (freezeUnitCoroutine != null)
            StopCoroutine(freezeUnitCoroutine);
        
        if(freezeTimerCoroutine != null)
            StopCoroutine(freezeTimerCoroutine);
        
        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
        GameManager.Instance.UnitSpawn.Controller.DeActivateFreeze();
    }

    private void SetTileStatusEffectInfo()
    {
        effectInfo = new TileStatusEffectInfo()
        {
            eBeginEffect = EEffectRcode.E_FreezeBegin,
            eAfterEffect = ECCType.Freeze,
            preparationTime = freezePreparationDuration,
            activeDuration = freezeDuration,
        };
    }

    private IEnumerator FreezeUnit()
    {
        UIManager.Instance.UI_Interface.ActivateBossSkillIndicator($"보스가 유닛을 얼립니다");
        freezeEffect.SetActive( true );
        
        GameManager.Instance.UnitSpawn.Controller.DeActivateFreeze();
        // canFreezeTiles.Clear();
                
        // 빙결 가능한 타일 개수 세기
        // foreach (UnitTile tile in GameManager.Instance.UnitSpawn.Controller.UnitTiles)
        // {
        //     if(!tile.IsFreeze)
        //         canFreezeTiles.Add(tile);
        // }

        SoundManager.Instance.PlayInGameSfx(EInGameSfx.InvincibleSkill);

        // if (canFreezeTiles.Count <= freezeTileCount)
        // {
        //     //전체 타일 빙결
        //     foreach (UnitTile tile in canFreezeTiles)
        //     {
        //         tile.SetStatutEffectUnitTile(effectInfo);
        //     }
        // }
        // else
        // {
        //     foreach(UnitTile tile in CollectionUtils.GetUniqueCollections(canFreezeTiles, freezeTileCount))
        //     {
        //         tile.SetStatutEffectUnitTile(effectInfo);
        //     }
        // }
        
        foreach(UnitTile tile in CollectionUtils.GetUniqueCollections(GameManager.Instance.UnitSpawn.Controller.UnitTiles, freezeTileCount))
        {
            tile.SetStatutEffectUnitTile(effectInfo);
        }
        
        yield return moveDelayWaitForSeconds;

        UIManager.Instance.UI_Interface.DeActivateBossSkillIndicator();
        freezeEffect.SetActive(false);

        boss.StateMachine.ChangeState(boss.StateMachine.MoveState);
        boss.SkillHandler.FinishSkillCast();
        StartCoroutine(SkillCoolTime());
        
        if (freezeUnitCoroutine != null)
        {
            StopCoroutine(freezeUnitCoroutine);
        }
        freezeTimerCoroutine = StartCoroutine(FreezeDurationCoroutine());
    }

    private IEnumerator FreezeDurationCoroutine()
    {
        yield return wait;
        GameManager.Instance.UnitSpawn.Controller.DeActivateFreeze();
    }
}
