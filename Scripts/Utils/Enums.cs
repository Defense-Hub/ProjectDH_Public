using System;
using GoogleSheet.Core.Type;

public enum EPoolObjectType
{
    // Unit,
    // Enemy,
    // Projectile,
    // Boss,
    // DamageTXT,
    // Unit2,
    // Unit3,
    // Unit4,
    // FreezeEffect_Begin,
    // Lava_Begin,
    // FreezeEffect_After = 101,
    // Lava_After,
    // Fire_Explosion_Delay,
    // Fire_Explosion_Active,
    // Meteor,
    // Meteor_Delay,
    // Meteor_Active,
    // Ice_Strike_Active,
    // Ground_Stun_Active,
    // Dark_Strike_Active,
    // Spawn_Enemy_Delay
}

public enum DEV_BossType
{
    WaterEnemy = EAddressableType.WaterEnemy,
    FireEnemy = EAddressableType.FireEnemy,
    IceEnemy = EAddressableType.IceEnemy,
    EarthEnemy = EAddressableType.EarthEnemy,
    DarkEnemy = EAddressableType.DarkEnemy,
}

public enum ECCType
{
    Default, // 0
    Slow, // 1
    Stun = 10000,
    Freeze,
    Lava,
}

[Flags]
public enum ESpecialAttackType
{
    Default = 0,
    Splash = 1 << 0,
    Stun = 1 << 1,
    Slow = 1 << 2,
}

public enum EBossSkill
{
    __WaterBoss = 0,
    SplitSkill,
    BlockUnitSpawn,
    __FireBoss = 100,
    LavaSkill,
    SpawnEnemy,
    __IceBoss = 200,
    FreezeUnit,
    InvincibleSkill,
    __GroundBoss = 300,
    Toughness,
    MeteorSkill,
    __DarkBoss = 400,
    TeleportSkill,
    ShuffleUnit
}

public enum ESkillType
{
    CoolTime,
    HealthChange,
    OneTime
}

public enum ESceneType
{
    StartScene,
    MainScene,
    TutorialScene,
    TitleLoadingScene,
    InGameLoadingScene,
    TransitionScene,
}

public enum EGachaType
{
    Common,
    Unique,
    Legendary,
}

public enum EGachaResult
{
    NoSpace = -3,
    NoMoney = -2,
    Fail = -1,
    Success,
}