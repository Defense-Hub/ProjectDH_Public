using UnityEngine;
using System.Collections;

public class Boss_Ice : Boss
{
    protected override void AddHealthEvent()
    {
        base.AddHealthEvent();
        HealthSystem.OnHealthChange += UIManager.Instance.UI_Interface.UpdateBossHPBarUI;
    }

    protected override void SubtractHealthEvent()
    {
        base.SubtractHealthEvent();
        HealthSystem.OnHealthChange -= UIManager.Instance.UI_Interface.UpdateBossHPBarUI;
    }
}