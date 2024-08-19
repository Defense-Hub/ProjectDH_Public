using UnityEngine;

public class GroundSpear : ActiveSkill
{
    [SerializeField] SpecialAttack specialAttack;
    private Effect effect;

    public override void UseSkill()
    {
        ActiveSkill();
    }

    private void ActiveSkill()
    {
        SingleHitByHealth(specialAttack);
        if(isTargetOn)
            InitEffect();
    }

    private void InitEffect()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.GroundSpear);

        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_GroundSpear).ReturnMyComponent<Effect>();
        effect.OnEnd += EndSkill;
        effect.transform.position = targetEnemy.transform.position + Vector3.up * 3;
        effect.transform.LookAt(targetEnemy.transform.position);
    }

    public override void EndSkill()
    {
        effect.OnEnd -= EndSkill;
        effect.gameObject.SetActive(false);
        base.EndSkill();
    }
}
