using System.Collections;
using UnityEngine;

public class FireFlamethrower : ActiveSkill
{
    [SerializeField] private float ticDelay;

    [SerializeField] private GameObject uiPrefab;
    private FireFlameUI ui;
    private Effect effect;

    private Coroutine attackCoroutine;
    private WaitForSeconds ticDelayTime;

    public override void UseSkill()
    {
        base.UseSkill();
        InitBtn();
        ticDelayTime = new WaitForSeconds(ticDelay);
    }

    // TODO targetIdx를 선택하는 UI 및 버튼 제작
    public void ActiveSkill(int targetIdx)
    {
        DisableBtns();
        InitEffect(targetIdx);

        // TODO : 몬스터들에게 피해를 주고 ParticleSystem이 꺼지면 Off 해주는 기능
        if(attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(ActiveSkillRoutine((targetIdx + 1) % GameManager.Instance.Stage.EnemyWayPoints.Length));
    }

    private IEnumerator ActiveSkillRoutine(int targetIdx)
    {
        while (true) 
        {
            LineHit(targetIdx);
            yield return ticDelayTime;
        }
    }

    private void InitEffect(int targetIdx)
    {
        Vector3 startPos = GameManager.Instance.Stage.EnemyWayPoints[targetIdx].position;
        Vector3 targetPos = GameManager.Instance.Stage.EnemyWayPoints[(targetIdx + 1) % GameManager.Instance.Stage.EnemyWayPoints.Length].position;

        SoundManager.Instance.PlayInGameSfx(EInGameSfx.FireFlameThrower);
        effect = GameManager.Instance.Pool.SpawnFromPool((int)EEffectRcode.E_FireFlamethrower).ReturnMyComponent<Effect>();
        effect.OnEnd += EndSkill;
        effect.transform.position = startPos;
        effect.gameObject.transform.LookAt(targetPos);
    }

    private void InitBtn()
    {
        GameObject tmp = Instantiate(uiPrefab);
        ui = tmp.GetComponent<FireFlameUI>();
        if (ui == null) return;
        ui.Init(this);
    }
    
    private void DisableBtns()
    {
        ui.Disable();
    }

    public override void EndSkill()
    {
        StopCoroutine(attackCoroutine);
        effect.OnEnd -= EndSkill;
        base.EndSkill();
        Debug.Log("끝!");
    }
}
