using System.Collections.Generic;
using UnityEngine;

public class GroundStun : PassiveSkill
{
    [Header("# Skill Data")]
    //[SerializeField] private float damage;
    [SerializeField] private SpecialAttack stunData;
 
    private float splashRange;
    private Vector3 targetPos;
    private List<Enemy> enemies;
    private Effect groundStunEffect;
    public override void UseSkill()
    {
        base.UseSkill();
        splashRange = stunData.Splash.SplashRange;
        targetPos = unit.TargetEnemy.transform.position;
        enemies = new List<Enemy>();
        unit.AnimationEventHandler.OnSkillEnd += EndSkill;
        unit.AnimationEventHandler.OnSkillAttack += SkillAttack;
    }

    private void SkillAttack()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.GroundStun);
        groundStunEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_GroundStun).ReturnMyComponent<Effect>();
        groundStunEffect.gameObject.transform.position = targetPos;
        SplashHit(splashRange, stunData);
    }

    public override void EndSkill()
    {
        base.EndSkill();
        unit.AnimationEventHandler.OnSkillEnd -= EndSkill;
        unit.AnimationEventHandler.OnSkillAttack -= SkillAttack;
    }
}