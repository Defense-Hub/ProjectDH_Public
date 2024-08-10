using UnityEngine.UI;
using UnityEngine;

public class HpBar : PoolObject
{
    [SerializeField] private Image fillImage;

    private Enemy enemy;
    private Camera MainCamera;
    private Vector3 amount;

    private void Awake()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }
    }
    private void Update()
    {
        SetHpBarPosition();
        RotateHpBarToCam();
    }

    public void InitalizeHpBar(Enemy enemy)
    {
        this.enemy = enemy;
        fillImage.fillAmount = 1;
        enemy.HealthSystem.OnHealthChange += UpdateHpBar;
        enemy.HealthSystem.OnDie += DeActivateHpBar;
    }

    public void SetPosition(Vector3 amount)
    {
        this.amount = amount;
    }

    private void DeActivateHpBar()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.Pool.poolTransforms[3]);
        if (enemy != null)
        {
            enemy.HealthSystem.OnHealthChange -= UpdateHpBar;
            enemy.HealthSystem.OnDie -= DeActivateHpBar;
            enemy = null;
        }
    }

    private void UpdateHpBar(float healthPercentage)
    {
        fillImage.fillAmount = healthPercentage;
    }

    private void RotateHpBarToCam()
    {
        transform.LookAt(transform.position - MainCamera.transform.forward, MainCamera.transform.up);
    }

    private void SetHpBarPosition()
    {
        if (enemy != null)
            transform.position = enemy.transform.position + amount;
    }

    public void SetHpBarScale(Vector3 scale)
    {
        if (enemy != null)
            transform.localScale = scale;
    }
}
