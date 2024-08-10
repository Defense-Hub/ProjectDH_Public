using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Texts
{
    [Header("UI_Interface")]
    TXT_Stage,
    TXT_Timer,
    TXT_MonsterUI,
    TXT_WBWarning,
    TXT_SummonGold,
    TXT_GemStone,
    TXT_Gold,
    TXT_BossSkill,

    [Header("UI_Gacha")]
    TXT_LvN,
    TXT_LvU,
    TXT_LvL,
    TXT_Normal,
    TXT_Unique,
    TXT_Legendary,
    TXT_Gacha_GemStone,

    [Header("UI_Reinforce")]
    TXT_Normal_Gold,
    TXT_Unique_Gold,
    TXT_Legendary_Gold,
    TXT_SummonRate_Gold,
    TXT_Reinforce_Gold,
    TXT_Reinforce_GemStone,

    TXT_Normal_Lv,
    TXT_Unique_Lv,
    TXT_Legendary_Lv,
    TXT_SummonRate_Lv,

    [Header("UI_StartScene")]
    TXT_PlayerName,
    TXT_MaxStageVal
}

public enum EImages
{
    [Header("UI_Gacha")]
    Image_Normal_Icon,
    Image_Unique_Icon,
    Image_Legendary_Icon,
}

public enum Objects
{
    [Header("UI_Interface")]
    Object_MonsterUI,
    Object_BossUI,
    Object_BossHpBarUI,
    Object_BossSkillIndicator,
}

public enum Buttons
{
    [Header("UI_Interface")]
    Btn_Settings,
    Btn_Gacha,
    Btn_Reinforce,
    Btn_Summon,
    Btn_Myth,
    Btn_Misson,

    [Header("UI_Gacha")]
    Btn_Normal,
    Btn_Unique,
    Btn_Legendary,
    Btn_Gacha_Close,

    [Header("UI_Reinforce")]
    Btn_Normal_Reinforce,
    Btn_Unique_Reinforce,
    Btn_Legendary_Reinforce,
    Btn_SummonRate_Reinforce,
    Btn_Reinforce_Close,

    [Header("UI_StartScene")]
    Btn_StartSceneSetting,
    Btn_Start,
    Btn_Quit,

    [Header("UI_Hunt")]
    Btn_Hunt,

    [Header("UI_HuntInfo")]
    Btn_HuntInfo_Close,
    Btn_StartHunt
}