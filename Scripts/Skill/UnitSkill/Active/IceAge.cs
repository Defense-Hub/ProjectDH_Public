using UnityEngine;

public class IceAge : ActiveSkill
{
    [SerializeField] private float fillTime;
    [SerializeField] private GameObject uiPrefab;
    
    public float FillTime => fillTime;

    public float DamageRate { get; set; }
    private Effect explodeEffect;
    private IceAgeUI ui;

    public override void UseSkill()
    {
        base.UseSkill();
        // TODO : 최대 폭발 범위까지 게이지 채우기
        InitUI();
    }

    private void InitUI()
    {
        GameObject tmp = Instantiate(uiPrefab);
        ui = tmp.GetComponent<IceAgeUI>();
        if (ui == null) return;
        ui.Init(this);
    }

    // 폭발 버튼, 시간 지나면 자동 폭발
    public void ActiveSkill()
    {
        DisableBtns();
        InitEffect();
        AllLineHit();
        //TODO : 카메라 쉐이킹 추가
    }

    private void InitEffect()
    {
        explodeEffect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_IceAge).ReturnMyComponent<Effect>();
        SoundManager.Instance.PlayInGameSfx(EInGameSfx.IceAge);
        explodeEffect.OnEnd += EndSkill;
    }

    private void DisableBtns()
    {
        ui.Disable();
    }

    public override void EndSkill()
    {
        explodeEffect.OnEnd -= EndSkill;
        base.EndSkill();
    }
}
