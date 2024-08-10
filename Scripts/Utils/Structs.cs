using System;

public struct CCInfo
{
    public ECCType ccType;
    public float duration;
    public Action callBack ;
}

public struct TileStatusEffectInfo
{
    public EEffectRcode eBeginEffect;
    public ECCType eAfterEffect;
    public float preparationTime;
    public float activeDuration;
}

public struct SkillInfo
{
    public ESkillType skillType;
    public float animSpeedMultiplier;
    public int animParameterHash;
    public Action onSkill;
}
