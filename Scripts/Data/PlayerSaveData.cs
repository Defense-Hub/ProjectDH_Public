using System;
using UnityEngine;

[Serializable]
public class AchievementData
{
    [field:SerializeField] public int AchievementNum { get; private set; }
    [field:SerializeField] public int Progress { get; private set; }
}

[Serializable]
public class SoundVolumeaveData
{
    public float MasterVolume;
    public float BgmVolume;
    public float SfxUIVolume;
    public float SfxInGameVolume;

    public SoundVolumeaveData()
    {
        // if (SoundManager.Instance != null)
        // {
        //     SetSoundVolume();
        // }
    }

    public SoundVolumeaveData(float masterVolume, float bgmVolume, float sfxUIVolume, float sfxInGameVolume)
    {
        MasterVolume = masterVolume;
        BgmVolume = bgmVolume;
        SfxUIVolume = sfxUIVolume;
        SfxInGameVolume = sfxInGameVolume;
    }

    public void SetSoundVolume()
    {
        MasterVolume = SoundManager.Instance.GetAudioMixerVolume(EAudioMixerType.Master);
        BgmVolume = SoundManager.Instance.GetAudioMixerVolume(EAudioMixerType.Bgm);
        SfxUIVolume = SoundManager.Instance.GetAudioMixerVolume(EAudioMixerType.SfxUI);
        SfxInGameVolume = SoundManager.Instance.GetAudioMixerVolume(EAudioMixerType.SfxInGame);   
    }
}

[Serializable]
public class PlayerSaveData
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public int Level { get; private set; }
    [field:SerializeField] public int Gold { get; private set; }
    [field:SerializeField] public int MaxStageLevel { get; private set; }
    [field:SerializeField] public  SoundVolumeaveData SoundVolumeSaveData{ get; private set; }
    // [field:SerializeField] public AchievementData[] AchievementData { get; private set; }
    

    public PlayerSaveData(string name, int level, int gold, int maxStageLevel,SoundVolumeaveData soundVolumeSaveData)
    {
        Name = name;
        Level = level;
        Gold = gold;
        MaxStageLevel = maxStageLevel;
        SoundVolumeSaveData = soundVolumeSaveData;
    }

    public void SetMaxStageLevel()
    {
        int curLevel = GameManager.Instance.Stage.CurStageLevel;

        MaxStageLevel = Mathf.Max(curLevel, MaxStageLevel);
    }
}