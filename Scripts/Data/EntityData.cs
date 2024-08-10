using System;
using UnityEngine;

[Serializable]
public class EntityData
{
    [Header("# Id")]
    public int ID;

    [Header("# 이름")]
    public string Name;

    [Header("# 이동속도")]
    public float BaseMoveSpeed;
}