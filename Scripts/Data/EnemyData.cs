using System;

[Serializable]
public class EnemyData
{
    public float MoveSpeed;
    public float MaxHealth;
    public float Toughness;
    
    public EnemyData(float moveSpeed, float maxHealth, float toughness)
    {
        MoveSpeed = moveSpeed;
        MaxHealth = maxHealth;
        Toughness = toughness;
    }

    public EnemyData GetEnemyData()
    {
        return new EnemyData(MoveSpeed, MaxHealth, Toughness);
    }
}