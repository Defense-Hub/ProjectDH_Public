using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitStatus : UI_Popup
{

    enum Texts
    {
        TXT_Character,
        TXT_Characteristic,
        TXT_AttackPower,
        TXT_AttackPower_Increase,
        TXT_AttackSpeed,
        TXT_AttackSpeed_Increase,
        TXT_Info

    }

    enum Images
    {
        Image_Unit,
        Image_Skill,
        Image_Character,
        Image_Skill_Box
    }

    private Unit unit;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
       
    }

    public void UpdateUnitInfo(Unit unit)
    {
        if (Get<TMP_Text>(0) == null)
        {
            Init();
        }
        
        this.unit = unit;
        
        UpdateUI();
    }

    private void UpdateUI()
    {
         // Debug.Log(unit);
        //부모함수에 넣으면 좋음
        GetText((int)Texts.TXT_Character).text = unit.DataHandler.Data.Name;
        GetText((int)Texts.TXT_Characteristic).text = unit.DataHandler.Data.UnitRank.ToString();
        GetText((int)Texts.TXT_AttackPower).text = unit.StatHandler.BaseStat.AttackPower.ToString();
        GetText((int)Texts.TXT_AttackSpeed).text = unit.StatHandler.BaseStat.AttackSpeed.ToString();
        GetImage((int)Images.Image_Character).sprite = GameDataManager.Instance.UnitBases[unit.Id].Thumbnail;
        GetText((int)Texts.TXT_Info).text = unit.DataHandler.Data.Description;

        CalculateAttackPowerIncrease();
        CalculateAttackSpeedIncrease();
        /*GetText((int)Texts.TXT_AttackPower_Increase).text = CalculateAttackPowerIncrease()*/
        /*GetText((int)Texts.TXT_AttackSpeed_Increase).text = CalculateAttackSpeedIncrease().ToString();*/
        
        if(unit.SkillHandler == null)
        {
            GetImage((int)Images.Image_Skill_Box).gameObject.SetActive(false);
            return;
        }
        GetImage((int)Images.Image_Skill_Box).gameObject.SetActive(true);
        GetImage((int)Images.Image_Skill).sprite = unit.SkillHandler.GetActiveSkillIcon();

        // Debug.Log(unit);
    }

    private void CalculateAttackPowerIncrease()
    {
        // 공격력 증가량 계산 로직
        float value = unit.StatHandler.CurrentStat.AttackPower - unit.StatHandler.BaseStat.AttackPower;
        if (value <= 0)
        {
            GetText((int)Texts.TXT_AttackPower_Increase).gameObject.SetActive(false);
        }
            
        else 
        {
            GetText((int)Texts.TXT_AttackPower_Increase).gameObject.SetActive(true);
            GetText((int)Texts.TXT_AttackPower_Increase).text = value.ToString();
        }
    }

    private void CalculateAttackSpeedIncrease()
    {
        // 공격속도 증가량 계산 로직 추가
        float value = unit.StatHandler.CurrentStat.AttackSpeed - unit.StatHandler.BaseStat.AttackSpeed;

        if(value <= 0)
        {
            GetText((int)Texts.TXT_AttackSpeed_Increase).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.TXT_AttackSpeed_Increase).gameObject.SetActive(true);
            GetText((int)Texts.TXT_AttackSpeed_Increase).text = value.ToString();
        }

    }
}