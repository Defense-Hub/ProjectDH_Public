using System.IO;
using UnityEngine;

public class PlayerDataSaveSystem
{
    private AESCrypto crypto;
    private string persistentpath;
    private string streamingpath;
    
    public PlayerDataSaveSystem()
    {
        crypto = new AESCrypto();
        persistentpath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        streamingpath = Path.Combine(Application.streamingAssetsPath, "PlayerData.json");
    }    

    public void DataSave(PlayerSaveData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true); // 데이터 직렬화

        string encryptedJson = crypto.EncryptString(json); // 직렬화 된 데이터 암호화

        File.WriteAllText(persistentpath, encryptedJson);
    }

    public PlayerSaveData DataLoad()
    {
        if (!File.Exists(persistentpath) )
        {
            if (!File.Exists(streamingpath))
            {
                Debug.Log("데이터가 존재하지 않습니다 !");
                return null;
            }
            else
            {
                return LoadJson(streamingpath);
            }
        }
        else
        {
            return LoadJson(persistentpath);
        }
    }

    private PlayerSaveData LoadJson(string path)
    {
        string encryptedJson = File.ReadAllText(path); // 암호화된 데이터 읽어옴
        string json = crypto.DecryptString(encryptedJson); // 복호화 및 역직렬화
        
        return JsonUtility.FromJson<PlayerSaveData>(json);  
    }
    
}