using UnityEngine;
using System.Collections.Generic;

public class IceStrike : PassiveSkill
{
    [Header("# Skill Data")]
    [SerializeField] private List<GameObject> weaponRenderers = new List<GameObject>();
    [SerializeField] private float splashRange;
    //[SerializeField] private float damage;
    
    private Vector3 targetPos;
    private List<Enemy> enemies;
    private Effect iceStrikeEffect;

    public override void UseSkill()
    {
        base.UseSkill();
        targetPos = unit.TargetEnemy.transform.position;
        enemies = new List<Enemy>();
        unit.AnimationEventHandler.OnSkillEnd += EndSkill;
        unit.AnimationEventHandler.OnSkillAttack += SkillAttack;
        ActiveWeapon();
    }

    private void ActiveWeapon()
    {
        foreach (GameObject weapon in weaponRenderers) 
        {
            weapon.SetActive(true);
        }
    }

    private void SkillAttack()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.IceStrike);
        iceStrikeEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_IceStrike).ReturnMyComponent<Effect>();
        iceStrikeEffect.gameObject.transform.position = targetPos;
        SplashHit(splashRange);
    }

    public override void EndSkill()
    {
        base.EndSkill();
        DeActiveWeapon();
        unit.AnimationEventHandler.OnSkillEnd -= EndSkill;
        unit.AnimationEventHandler.OnSkillAttack -= SkillAttack;
    }

    private void DeActiveWeapon()
    {
        foreach (GameObject weapon in weaponRenderers)
        {
            weapon.SetActive(false);
        }
    }
}
