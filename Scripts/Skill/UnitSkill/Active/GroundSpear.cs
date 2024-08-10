using UnityEngine;

public class GroundSpear : ActiveSkill
{
    [SerializeField] SpecialAttack specialAttack;
    private Effect effect;

    public override void UseSkill()
    {
        base.UseSkill();
        ActiveSkill();
    }

    private void ActiveSkill()
    {
        SingleHitByHealth(specialAttack);
        InitEffect();
        EndSkill();
    }

    private void InitEffect()
    {
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.GroundSpear);

        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_GroundSpear).ReturnMyComponent<Effect>();
        effect.transform.position = targetEnemy.transform.position + Vector3.up * 3;
        effect.transform.LookAt(targetEnemy.transform.position);
    }

    public override void EndSkill()
    {
        base.EndSkill();
    }
}
