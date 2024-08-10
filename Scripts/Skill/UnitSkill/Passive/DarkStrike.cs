using System.Collections.Generic;
using UnityEngine;

public class DarkStrike : PassiveSkill
{
    [Header("# Skill Data")]
    [SerializeField][Range(1f, 5f)] private int AttackCount;
    [SerializeField][Range(1f, 10f)] private float AttackSpeedRate;
    [SerializeField] private float splashRange;
    //[SerializeField] private float damage;

    private Vector3 targetPos;
    private List<Enemy> enemies;
    private Effect darkStrikeEffect;
    private int curCount;
    public override void UseSkill()
    {
        base.UseSkill();
        curCount = 0;
        targetPos = unit.TargetEnemy.transform.position;
        enemies = new List<Enemy>();
        unit.AnimationEventHandler.OnSkillCount += CountSkill;
        unit.AnimationEventHandler.OnSkillEnd += EndSkill;
        unit.AnimationEventHandler.OnSkillAttack += SkillAttack;
    }

    private void SkillAttack()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.DarkStrike);
        darkStrikeEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_DarkStrike).ReturnMyComponent<Effect>();
        darkStrikeEffect.gameObject.transform.position = targetPos;
        SplashHit(splashRange);
    }

    private void CountSkill()
    {
        curCount++;
        Debug.Log($"{curCount}"); // 0 1 2
        if(curCount == AttackCount)
        {
            unit.AnimationEventHandler.CallSkillEnd();
        }
    }

    public override void EndSkill()
    {
        base.EndSkill();
        unit.AnimationEventHandler.OnSkillCount -= CountSkill;
        unit.AnimationEventHandler.OnSkillEnd -= EndSkill;
        unit.AnimationEventHandler.OnSkillAttack -= SkillAttack;
    }
}
