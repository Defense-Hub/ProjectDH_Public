using System.Collections.Generic;
using UnityEngine;

public class WaterSlide : PassiveSkill
{
    private List<Enemy> enemies;
    private Effect effect;

    public override void UseSkill()
    {
        base.UseSkill();
        enemies = new List<Enemy>();
        unit.AnimationEventHandler.OnSkillEnd += EndSkill;
        unit.AnimationEventHandler.OnSkillAttack += SkillAttack;
    }

    private void SkillAttack()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.WaterSlide);
        //TODO : 적절한 waterSlideEffect 적용
        int targetIdx = unit.TargetEnemy.TargetIndex;
        InitEffect(targetIdx);
        LineHit(targetIdx);
    }

    private void InitEffect(int targetIdx)
    {
        Vector3 startPos = GameManager.Instance.Stage.EnemyWayPoints[(targetIdx + 3) % 4].position + Vector3.up;
        Vector3 targetPos = GameManager.Instance.Stage.EnemyWayPoints[targetIdx].position + Vector3.up;

        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_WaterSlide).ReturnMyComponent<Effect>();
        effect.transform.position = startPos;
        effect.transform.LookAt(targetPos);
    }

    public override void EndSkill()
    {
        base.EndSkill();
        unit.AnimationEventHandler.OnSkillEnd -= EndSkill;
        unit.AnimationEventHandler.OnSkillAttack -= SkillAttack;
    }
}
