using System.Collections;
using UnityEngine;

public class HuntEnemy : Enemy
{
    protected override void EnemyDie()
    {
        GameManager.Instance.System.SetHuntMissionDelay();
        if (HealthSystem.IsDie())
        {
            GameManager.Instance.System.Mission.DieHuntEnemy();   
        }
        
        if (dieCoroutine != null)
        {
            StopCoroutine(dieCoroutine);
        }
        dieCoroutine = StartCoroutine(WaitForDieAnim());
    }

    public override void EnemyInit(int level)
    {
        EnemyData = new EnemyData(GameDataManager.Instance.HuntEnemyDatas[level].MoveSpeed,
            GameDataManager.Instance.HuntEnemyDatas[level].MaxHealth,
            GameDataManager.Instance.HuntEnemyDatas[level].Toughness);

        SetEnemyState();

        UIManager.Instance.UI_EnemyHpBar.SetHpBarToHuntEnemy(this);
    }
    
    private IEnumerator WaitForDieAnim()
    {
        Animator.SetTrigger(AnimationData.DieParameterHash);
        if (dieAnimLength == null)
        {
            dieAnimLength = new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        yield return dieAnimLength;
        
        gameObject.SetActive(false);
        SubtractHealthEvent();
    }
}