using System;

[Serializable]
public class EnemyStat
{
    public float MoveSpeed;
    public float MaxHealth;
    public float Toughness;

    public EnemyStat(float moveSpeed, float maxHealth, float toughness)
    {
        MoveSpeed = moveSpeed;
        MaxHealth = maxHealth;
        Toughness = toughness;
    }
}
