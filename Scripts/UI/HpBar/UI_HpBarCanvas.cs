using UnityEngine;

public class UI_HpBarCanvas : MonoBehaviour
{
    private readonly Vector3 normalScale = new Vector3(1f, 0.13f, 1f);
    private readonly Vector3 huntScale = new Vector3(2f, 0.22f, 1f);

    private void Start()
    {
        UIManager.Instance.UI_EnemyHpBar = this;
        UIManager.Instance.WorldCanvasTransform = transform;
    }

    public void SetHpBarToNormalEnemy(Enemy enemy)
    {
        HpBar hpBar = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_HpBar).ReturnMyComponent<HpBar>();
        hpBar.transform.SetParent(transform);
        hpBar.InitalizeHpBar(enemy);

        hpBar.SetHpBarScale(normalScale);
        hpBar.SetPosition(Vector3.up * 1.4f + Vector3.back * -0.1f + Vector3.right * 0.05f);
    }

    public void SetHpBarToHuntEnemy(Enemy enemy)
    {
        HpBar hpBar = GameManager.Instance.Pool.SpawnFromPool((int)EOhterRcode.O_HpBar).ReturnMyComponent<HpBar>();
        hpBar.transform.SetParent(transform);
        hpBar.InitalizeHpBar(enemy);

        hpBar.SetHpBarScale(huntScale);
        hpBar.SetPosition(Vector3.up * 3f + Vector3.back * -0.1f + Vector3.right * 0.05f);
    }
}
