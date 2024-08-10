public enum EAddressableType
{
    Unit,
    UI,
    Effect,
    Other,
    WaterEnemy,
    FireEnemy,
    IceEnemy,
    EarthEnemy,
    DarkEnemy,
    HuntEnemy,
    Map,
    Thumbnail,
}

public enum EEnemyType
{
    Basic = 1000,
    Boss,
}

public enum EMapRcode
{
    M_Water,
    M_Fire,
    M_Ice,
    M_Earth,
    M_Dark,
}

public enum EOhterRcode
{
    O_Projectile=100000,
    O_DamageText,
    O_Meteo,
    O_HpBar,
    O_GachaResult,
}
public enum EEffectRcode
{
    E_FreezeBegin = 10000,
    E_FreezeAfter ,
    E_LavaAfter ,
    E_LavaBegin ,
    E_MeteoBegin ,
    E_MeteoActive ,
    E_IceStrike,
    E_GroundStun,
    E_DarkStrike,
    E_EnemySpawnDelay,
    E_FireExplosion,
    E_ExRangeIndicator,
    E_FireFlamethrower,
    E_EntityStun,
    E_EntitySlow,
    E_EntityFreeze,
    E_EntityLava,
    E_IceAge,
    E_GroundSpear,
    E_DarkSpear,
    E_WaterTornado,
    E_WaterSlide,
}
public enum EFireEnemyRCode
{
    FE001 = 1000,
    FB002,
}

public enum EWaterEnemyRCode
{ 
    WE001 = 1000,
    WB002,
}

public enum EEarthEnemyRCode
{ 
    GE001 = 1000,
    GB002,
}

public enum EIceEnemyRCode
{ 
    IE001 = 1000,
    IB002,
}
public enum EDarkEnemyRCode
{ 
    DE001 = 1000,
    DB002,
}

public enum EHuntEnemyCode
{ 
    DH001 
}

public enum EUnitRCode
{
    FC001 = 0,
    IC001,
    GC001,
    DC001,
    WC001,
    FR001 = 100,
    IR001,
    GR001,
    DR001,
    WR001,
    FU001 = 200,
    IU001,
    GU001,
    DU001,
    WU001,
    FL001 = 300,
    IL001,
    GL001,
    DL001,
    WL001,
    FEP001 = 400,
    IEP001,
    GEP001,
    DEP001,
    WEP001,
}

public enum EUIRCode
{
    UI_UnitStatus,
    UI_Settings,
    UI_Gacha,
    UI_Reinforce,
    UI_Mission,
    UI_Myth,
    UI_NameInput,
    UI_GameOver,
    UI_GameClear,
    UI_Summon,
    UI_Hunt,
    UI_HuntInfo
}