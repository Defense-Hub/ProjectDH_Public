using UnityEngine;

[CreateAssetMenu(menuName ="Entity/Enemy", fileName ="EnemyBaseSO", order = 0)]
public class EnemyBaseSO : EntityBaseSO
{
    [Header("# 최대 체력")]
    public float BaseHealth;

    [Header("# 강인함")]
    [Range(0f, 100f)] public float Toughness;
}