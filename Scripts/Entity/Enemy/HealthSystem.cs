using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float curHealth;

    public event Action<float, Transform> OnHit;
    public event Action OnDie;
    public event Action<float> OnHealthChange;

    private bool isDie;
    private bool isInvincible;
    private Transform wp;

    public void InitHealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
        curHealth = this.maxHealth;
        isDie = false;
        ChangeInvincibility(false);
        CallHealthChangeEvent(GetHealthPercentage());
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            Debug.Log("무적!");
            CallHitEvent(0, wp);
            return;
        }
        curHealth = Mathf.Max(curHealth - damage, 0);

        if (!isDie && curHealth == 0)
        {
            CallDieEvent();
        }
        CallHitEvent(damage, wp);
        CallHealthChangeEvent(GetHealthPercentage());
    }

    public void CallHitEvent(float damage, Transform transform)
    {
        OnHit?.Invoke(damage, transform);
    }

    public void CallDieEvent()
    {
        isDie = true;
        OnDie?.Invoke();
    }

    public void CallHealthChangeEvent(float healthPercentage)
    {
        OnHealthChange?.Invoke(healthPercentage);
    }

    public void ChangeInvincibility(bool b)
    {
        isInvincible = b;
    }

    public float GetHealthPercentage()
    {
        return curHealth / maxHealth;
    }

    public int GetCurHealth()
    {
        return (int)curHealth;
    }

    public void SetWayPoint(Transform transform)
    {
        wp = transform;
    }

    public bool IsDie()
    {
        return isDie;
    }
}
