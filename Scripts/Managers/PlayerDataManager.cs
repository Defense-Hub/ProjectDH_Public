using System;
using System.Threading.Tasks;
using UGS;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [field: Header("# In Game Player Data Info")]
     public PlayerInGameData inGameData { get; private set; }

    [field: Header("# Player Save Data Info")]
    public PlayerSaveData SaveData { get; private set; }
    
    private PlayerDataSaveSystem saveSystem;

    public int SpawnLevel => inGameData.SpawnProbabilityLevel - 1;

    public event Action OnUISetting;
    
    protected override void Awake()
    {
        base.Awake();
        saveSystem = new PlayerDataSaveSystem();
    }
    
    public void LoadPlayerData()
    {
        SaveData =  saveSystem.DataLoad();

        if (SaveData != null)
        {
            SoundManager.Instance.SetAudioMixerVolume(SaveData.SoundVolumeSaveData);
        }
    }

    public void SetPlayerData(string name)
    {
        SaveData = new PlayerSaveData(name, 1, 0, 0, new SoundVolumeaveData());
        SavePlayerData();
    }

    public void CreateInGameData()
    {
        inGameData = new PlayerInGameData();
    }

    public void CallPlayerDataUISetting()
    {
        OnUISetting?.Invoke();
    }

    public void ResetEvent()
    {
        OnUISetting = null;
    }
    public void SavePlayerData()
    {
        if (GameManager.Instance.Stage != null)
        {
            SaveData.SetMaxStageLevel();
        }
        
        SaveData.SoundVolumeSaveData.SetSoundVolume();
        
        saveSystem.DataSave(SaveData);
    }
}