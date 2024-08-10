using UnityEngine;

[CreateAssetMenu(menuName = "Entity/default", fileName = "EntityBaseSO", order = 0)]
public class EntityBaseSO : ScriptableObject
{
    [Header("# Id")]
    public int Id;

    [Header("# 이름")]
    public string Name;

    [Header("# 이동속도")]
    public float BaseMoveSpeed;
}

