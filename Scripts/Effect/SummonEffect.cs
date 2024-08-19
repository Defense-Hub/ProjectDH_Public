using UnityEngine;

public class SummonEffect : Effect
{
    [SerializeField] private ParticleSystem lightParticle;
    [SerializeField] private ParticleSystem sideParticle;

    public void SetParticleColor(EUnitRank unitRank)
    {
        var light = lightParticle.main;
        var side = sideParticle.main;
        switch (unitRank)
        {
            case EUnitRank.Common:
                light.startColor = GameDataManager.Instance.CommonUnitColor;
                side.startColor = GameDataManager.Instance.CommonUnitColor;
                break;

            case EUnitRank.Rare:
                light.startColor = GameDataManager.Instance.RareUnitColor;
                side.startColor = GameDataManager.Instance.RareUnitColor;
                break;

            case EUnitRank.Unique:
                light.startColor = GameDataManager.Instance.UniqueUnitColor;
                side.startColor = GameDataManager.Instance.UniqueUnitColor;
                break;

            case EUnitRank.Legendary:
                light.startColor = GameDataManager.Instance.LegendaryUnitColor;
                side.startColor = GameDataManager.Instance.LegendaryUnitColor;
                break;
            case EUnitRank.Epic:
                light.startColor = GameDataManager.Instance.EpicUnitColor;
                side.startColor = GameDataManager.Instance.EpicUnitColor;
                break;
        }
    }
}
